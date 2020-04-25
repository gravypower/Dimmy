using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;

namespace Dimmy.Engine.Commands.Plugins
{
    public class InstallPluginCommandHandler:ICommandHandler<InstallPlugin>
    {
        private readonly ILogger _logger;

        public InstallPluginCommandHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task Handle(InstallPlugin command)
        {
            return Task.Run(() => Run(command));
        }

        private async Task Run(InstallPlugin command)
        {
            var pluginInstallFolder = Path.Combine(command.InstallDirectory, command.PackageId);

            if (Directory.Exists(pluginInstallFolder))
                return;

            Directory.CreateDirectory(pluginInstallFolder);

            var packageVersion = NuGetVersion.Parse(command.PackageVersion);
            var nuGetFramework = NuGetFramework.ParseFolder("netstandard2.1");
            var settings = Settings.LoadDefaultSettings(root: null);

            var packageSourceProvider = new PackageSourceProvider(settings);

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, Repository.Provider.GetCoreV3());

            using var cacheContext = new SourceCacheContext();

            var repositories = sourceRepositoryProvider.GetRepositories();
            var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);

            await GetPackageDependencies(
                new PackageIdentity(command.PackageId, packageVersion),
                nuGetFramework, 
                cacheContext, 
                _logger, 
                repositories, 
                availablePackages,
                command.OmitDependencies);

            var resolverContext = new PackageResolverContext(
                DependencyBehavior.Lowest,
                new[] { command.PackageId },
                Enumerable.Empty<string>(),
                Enumerable.Empty<PackageReference>(),
                Enumerable.Empty<PackageIdentity>(),
                availablePackages,
                sourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
                _logger);

            var resolver = new PackageResolver();
            var packagesToInstall = resolver.Resolve(resolverContext, CancellationToken.None)
                .Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p)));
            var packagePathResolver = new PackagePathResolver(Path.GetFullPath("packages"));
            var packageExtractionContext =
                new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    XmlDocFileSaveMode.None,
                    ClientPolicyContext.GetClientPolicy(settings, _logger),
                    _logger);

            var frameworkReducer = new FrameworkReducer();

            foreach (var packageToInstall in packagesToInstall)
            {
                PackageReaderBase packageReader;
                var installedPath = packagePathResolver.GetInstalledPath(packageToInstall);
                if (installedPath == null)
                {
                    var downloadResource =
                        await packageToInstall.Source.GetResourceAsync<DownloadResource>(CancellationToken.None);
                    var downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                        packageToInstall,
                        new PackageDownloadContext(cacheContext),
                        SettingsUtility.GetGlobalPackagesFolder(settings),
                        _logger,
                        CancellationToken.None);

                    await PackageExtractor.ExtractPackageAsync(
                        downloadResult.PackageSource,
                        downloadResult.PackageStream,
                        packagePathResolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    packageReader = downloadResult.PackageReader;
                    installedPath = packagePathResolver.GetInstalledPath(packageToInstall);
                }
                else
                {
                    packageReader = new PackageFolderReader(installedPath);
                }


                var libItems = packageReader.GetLibItems().ToList();
                var nearest = frameworkReducer.GetNearest(nuGetFramework, libItems.Select(x => x.TargetFramework));
                var assemblies = new List<string>();

                assemblies.AddRange(libItems
                    .Where(x => x.TargetFramework.Equals(nearest))
                    .SelectMany(x => x.Items));

                foreach (var assembly in assemblies)
                {
                    var destFileName = Path.Combine(pluginInstallFolder, Path.GetFileName(assembly));

                    if (File.Exists(destFileName)) continue;

                    var sourceFileName = Path.Combine(installedPath, assembly);
                    File.Copy(sourceFileName, destFileName);
                }
            }
        }

        async Task GetPackageDependencies(
            PackageIdentity package,
            NuGetFramework framework,
            SourceCacheContext cacheContext,
            ILogger logger,
            IEnumerable<SourceRepository> repositories,
            ISet<SourcePackageDependencyInfo> availablePackages,
            string[] omitDependencies)
        {
            if (availablePackages.Contains(package)) return;

            foreach (var sourceRepository in repositories)
            {
                var dependencyInfoResource = await sourceRepository.GetResourceAsync<DependencyInfoResource>();
                var dependencyInfo = await dependencyInfoResource.ResolvePackage(
                    package, framework, cacheContext, _logger, CancellationToken.None);

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
                    await GetPackageDependencies(
                        new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion),
                        framework, 
                        cacheContext, 
                        _logger, 
                        repositories, 
                        availablePackages,
                        omitDependencies);
                }
            }
        }
    }
}
