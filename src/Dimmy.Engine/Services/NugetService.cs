using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dimmy.Engine.NuGet;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;

namespace Dimmy.Engine.Services
{
    public class NugetService : INugetService
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<SourceRepository> _repositories;
        private readonly ISettings _settings;
        private readonly SourceCacheContext _sourceCacheContext;

        public NugetService(
            SourceCacheContext sourceCacheContext,
            ISettings settings,
            ILogger logger)
        {
            _sourceCacheContext = sourceCacheContext;
            _settings = settings;
            _logger = logger;

            var packageSourceProvider = new PackageSourceProvider(settings);
            var sourceRepositoryProvider =
                new SourceRepositoryProvider(packageSourceProvider, Repository.Provider.GetCoreV3());

            _repositories = sourceRepositoryProvider.GetRepositories();
        }


        public async IAsyncEnumerable<IPackageSearchMetadata> GetNugetPackagesFromTag(string tag)
        {
            foreach (var sourceRepository in _repositories)
            {
                var searchResource = await sourceRepository.GetResourceAsync<PackageSearchResource>();
                var searchMetadata = await searchResource.SearchAsync(
                    @$"Tags:""{tag}""",
                    new SearchFilter(true),
                    0,
                    50,
                    new TextWriterLogger(Console.Out),
                    CancellationToken.None);

                foreach (var packageSearchMetadata in searchMetadata) yield return packageSearchMetadata;
            }
        }

        public async Task<IEnumerable<SourcePackageDependencyInfo>> GetPackageAndDependencies(
            PackageIdentity packageId,
            NuGetFramework packageFramework,
            string[] omitDependencies)
        {
            var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);

            await DoGetPackageDependencies(packageId);

            return availablePackages;


            async Task DoGetPackageDependencies(PackageIdentity p)
            {
                if (availablePackages.Contains(p)) return;

                foreach (var sourceRepository in _repositories)
                {
                    var dependencyInfoResource = await sourceRepository.GetResourceAsync<DependencyInfoResource>();
                    var dependencyInfo = await dependencyInfoResource.ResolvePackage(p, packageFramework,
                        _sourceCacheContext, _logger, CancellationToken.None);

                    if (dependencyInfo == null) continue;


                    if (omitDependencies.Any() && dependencyInfo.Dependencies.Any(d => omitDependencies.Contains(d.Id)))
                    {
                        var packageDependencies =
                            dependencyInfo.Dependencies
                                .Where(d => !omitDependencies.Contains(d.Id))
                                .Select(d => d);

                        dependencyInfo = new SourcePackageDependencyInfo(
                            dependencyInfo.Id,
                            dependencyInfo.Version,
                            packageDependencies,
                            dependencyInfo.Listed,
                            dependencyInfo.Source);
                    }


                    availablePackages.Add(dependencyInfo);

                    foreach (var dependency in dependencyInfo.Dependencies)
                    {
                        var packageIdentity = new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion);
                        await DoGetPackageDependencies(packageIdentity);
                    }
                }
            }
        }

        public async Task<IEnumerable<SourcePackageDependencyInfo>> GetPackagesToInstall(
            PackageIdentity packageId,
            NuGetFramework packageFramework,
            string[] omitDependencies)
        {
            var availablePackages = await GetPackageAndDependencies(packageId, packageFramework, omitDependencies);

            var sourcePackageDependencyInfos =
                availablePackages as SourcePackageDependencyInfo[] ?? availablePackages.ToArray();

            var resolverContext = new PackageResolverContext(
                DependencyBehavior.Lowest,
                new[] {packageId.Id},
                Enumerable.Empty<string>(),
                Enumerable.Empty<PackageReference>(),
                Enumerable.Empty<PackageIdentity>(),
                sourcePackageDependencyInfos,
                _repositories.Select(s => s.PackageSource),
                _logger);

            var resolver = new PackageResolver();
            return resolver.Resolve(resolverContext, CancellationToken.None)
                .Select(p => sourcePackageDependencyInfos.Single(x => PackageIdentityComparer.Default.Equals(x, p)));
        }

        public async Task<(PackageReaderBase package, string installPath)> DownloadPackage(
            SourcePackageDependencyInfo packageToInstall)
        {
            var packagePathResolver = new PackagePathResolver(Path.GetFullPath("packages"));
            var packageExtractionContext =
                new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    XmlDocFileSaveMode.None,
                    ClientPolicyContext.GetClientPolicy(_settings, _logger),
                    _logger);

            var installedPath = packagePathResolver.GetInstalledPath(packageToInstall);

            if (installedPath != null) return (new PackageFolderReader(installedPath), installedPath);

            var downloadResource =
                await packageToInstall.Source.GetResourceAsync<DownloadResource>(CancellationToken.None);

            var downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                packageToInstall,
                new PackageDownloadContext(_sourceCacheContext),
                SettingsUtility.GetGlobalPackagesFolder(_settings),
                _logger,
                CancellationToken.None);

            await PackageExtractor.ExtractPackageAsync(
                downloadResult.PackageSource,
                downloadResult.PackageStream,
                packagePathResolver,
                packageExtractionContext,
                CancellationToken.None);

            installedPath = packagePathResolver.GetInstalledPath(packageToInstall);

            return (downloadResult.PackageReader, installedPath);
        }
    }
}