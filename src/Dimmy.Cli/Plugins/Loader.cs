using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dimmy.Cli.Application;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.Signing;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Resolver;
using NuGet.Versioning;

namespace Dimmy.Cli.Plugins
{
    public class Loader
    {
        private static readonly string PluginsDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        private static readonly DirectoryInfo PluginsDirectory = new DirectoryInfo(PluginsDirectoryPath);
        public async Task Load()
        {
            var logger = new TextWriterLogger(Console.Out);
            var packageId = "Dimmy.Sitecore.Plugin";
            var packageVersion = NuGetVersion.Parse("1.0.0");
            var nuGetFramework = NuGetFramework.ParseFolder("netstandard2.1");
            var settings =  Settings.LoadDefaultSettings(root: null);

            var packageSourceProvider = new PackageSourceProvider(
                settings, new PackageSource[] { new PackageSource("http://localhost:8624/nuget/localfeed") });

            var sourceRepositoryProvider = new SourceRepositoryProvider(packageSourceProvider, Repository.Provider.GetCoreV3());

            using var cacheContext = new SourceCacheContext();

            var repositories = sourceRepositoryProvider.GetRepositories();
            var availablePackages = new HashSet<SourcePackageDependencyInfo>(PackageIdentityComparer.Default);
            await GetPackageDependencies(
                new PackageIdentity(packageId, packageVersion),
                nuGetFramework, cacheContext, logger, repositories, availablePackages);

            var resolverContext = new PackageResolverContext(
                DependencyBehavior.Lowest,
                new[] { packageId },
                Enumerable.Empty<string>(),
                Enumerable.Empty<PackageReference>(),
                Enumerable.Empty<PackageIdentity>(),
                availablePackages,
                sourceRepositoryProvider.GetRepositories().Select(s => s.PackageSource),
                logger);

            var resolver = new PackageResolver();
            var packagesToInstall = resolver.Resolve(resolverContext, CancellationToken.None)
                .Select(p => availablePackages.Single(x => PackageIdentityComparer.Default.Equals(x, p)));
            var packagePathResolver = new PackagePathResolver(Path.GetFullPath("packages"));
            var packageExtractionContext =
                new PackageExtractionContext(
                    PackageSaveMode.Defaultv3,
                    XmlDocFileSaveMode.None,
                    ClientPolicyContext.GetClientPolicy(settings, logger),
                    logger);

            var frameworkReducer = new FrameworkReducer();

            foreach (var packageToInstall in packagesToInstall)
            {
                PackageReaderBase packageReader;
                var installedPath = packagePathResolver.GetInstalledPath(packageToInstall);
                if (installedPath == null)
                {
                    var downloadResource = await packageToInstall.Source.GetResourceAsync<DownloadResource>(CancellationToken.None);
                    var downloadResult = await downloadResource.GetDownloadResourceResultAsync(
                        packageToInstall,
                        new PackageDownloadContext(cacheContext),
                        PluginsDirectoryPath,
                        logger, 
                        CancellationToken.None);

                    await PackageExtractor.ExtractPackageAsync(
                        downloadResult.PackageSource,
                        downloadResult.PackageStream,
                        packagePathResolver,
                        packageExtractionContext,
                        CancellationToken.None);

                    packageReader = downloadResult.PackageReader;
                }
                else
                {
                    packageReader = new PackageFolderReader(installedPath);
                }

                var libItems = packageReader.GetLibItems();
                var nearest = frameworkReducer.GetNearest(nuGetFramework, libItems.Select(x => x.TargetFramework));
                Console.WriteLine(string.Join("\n", libItems
                    .Where(x => x.TargetFramework.Equals(nearest))
                    .SelectMany(x => x.Items)));

                var frameworkItems = packageReader.GetFrameworkItems();
                nearest = frameworkReducer.GetNearest(nuGetFramework, frameworkItems.Select(x => x.TargetFramework));
                Console.WriteLine(string.Join("\n", frameworkItems
                    .Where(x => x.TargetFramework.Equals(nearest))
                    .SelectMany(x => x.Items)));
            }


            //var logger = new TextWriterLogger(Console.Out);
            //var repository = Repository.Factory.GetCoreV3(@"https://api.nuget.org/v3/index.json");
            //var packageMetadataResource = repository.GetResourceAsync<PackageMetadataResource>().Result;
            //var sourceCacheContext = new SourceCacheContext();

            //foreach (var nupkgFile in PluginsDirectory.GetFiles())
            //{
            //    var pluginDirectoryPath = Path.Combine(PluginsDirectoryPath, Path.GetFileNameWithoutExtension(nupkgFile.Name));
            //    //if (Directory.Exists(path)) continue;

            //    var nupkgFileStream = File.Open(nupkgFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            //    var pluginPackageArchiveReader = new PackageArchiveReader(nupkgFileStream);

            //    Directory.CreateDirectory(pluginDirectoryPath);

            //    var settings = Settings.LoadDefaultSettings(root: null);

            //    var packageExtractionContext =
            //        new PackageExtractionContext(
            //            PackageSaveMode.Defaultv3,
            //            XmlDocFileSaveMode.None,
            //            ClientPolicyContext.GetClientPolicy(settings, logger),
            //            logger);

            //    await PackageExtractor.ExtractPackageAsync(nupkgFile.FullName, pluginPackageArchiveReader,
            //        nupkgFileStream, new PackagePathResolver(PluginsDirectoryPath), packageExtractionContext,
            //        CancellationToken.None);
            //    //var packageAssemblies = pluginPackageArchiveReader
            //    //    .GetFiles()
            //    //    .Where(p => Path.GetExtension(p) == ".dll");

            //    //foreach (var packageAssembly in packageAssemblies)
            //    //{
            //    //    var savePath = Path.Combine(pluginDirectoryPath, Path.GetFileName(packageAssembly));
            //    //    using var assemblyStream = pluginPackageArchiveReader.GetStream(packageAssembly);
            //    //    using var file = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            //    //    assemblyStream.CopyTo(file);
            //    //}



            //    //var dependencies = pluginPackageArchiveReader.GetPackageDependencies()
            //    //    .SelectMany(item => item.Packages)
            //    //    .ToList();


            //    //foreach (var dependency in dependencies)
            //    //{

            //    //    if(dependency.Id.StartsWith("Dimmy"))
            //    //        continue;

            //    //    var dependencyPackageIdentity = new PackageIdentity(dependency.Id, NuGetVersion.Parse(dependency.VersionRange.OriginalString));

            //    //    var dependencyMetadata = packageMetadataResource
            //    //        .GetMetadataAsync(dependencyPackageIdentity, sourceCacheContext, logger, CancellationToken.None).Result;

            //    //    var finder = repository.GetResourceAsync<FindPackageByIdResource>().Result;

            //    //    await using var dependencyMemoryStream = new MemoryStream();
            //    //    if (await finder.CopyNupkgToStreamAsync(dependencyMetadata.Identity.Id,
            //    //        dependencyMetadata.Identity.Version, dependencyMemoryStream, sourceCacheContext, logger,
            //    //        CancellationToken.None))
            //    //    {
            //    //        var dependencyPackageArchiveReader = new PackageArchiveReader(dependencyMemoryStream);

            //    //        dependencyPackageArchiveReader.

            //    //    }

            //    //}
            //}
        }

        async Task GetPackageDependencies(PackageIdentity package,
            NuGetFramework framework,
            SourceCacheContext cacheContext,
            ILogger logger,
            IEnumerable<SourceRepository> repositories,
            ISet<SourcePackageDependencyInfo> availablePackages)
        {
            if (availablePackages.Contains(package)) return;

            foreach (var sourceRepository in repositories)
            {
                var dependencyInfoResource = await sourceRepository.GetResourceAsync<DependencyInfoResource>();
                var dependencyInfo = await dependencyInfoResource.ResolvePackage(
                    package, framework, cacheContext, logger, CancellationToken.None);

                if (dependencyInfo == null) continue;

                availablePackages.Add(dependencyInfo);
                foreach (var dependency in dependencyInfo.Dependencies)
                {
                    await GetPackageDependencies(
                        new PackageIdentity(dependency.Id, dependency.VersionRange.MinVersion),
                        framework, cacheContext, logger, repositories, availablePackages);
                }
            }
        }
    }
}
