using System.IO;
using System.Linq;
using Dimmy.Engine.Services;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace Dimmy.Engine.Commands.Plugins
{
    public class InstallPluginCommandHandler : ICommandHandler<InstallPlugin>
    {
        private readonly INugetService _nugetService;

        public InstallPluginCommandHandler(INugetService nugetService)
        {
            _nugetService = nugetService;
        }

        public async void Handle(InstallPlugin command)
        {
            var pluginInstallFolder = Path.Combine(command.InstallDirectory, command.PackageId);

            if (Directory.Exists(pluginInstallFolder))
                return;

            Directory.CreateDirectory(pluginInstallFolder);

            var nuGetVersion = NuGetVersion.Parse(command.PackageVersion);
            var packageIdentity = new PackageIdentity(command.PackageId, nuGetVersion);
            var nuGetFramework = NuGetFramework.ParseFolder(command.PackageFramework);

            var packagesToInstall = await _nugetService
                .GetPackagesToInstall(packageIdentity, nuGetFramework, command.OmitDependencies);

            var frameworkReducer = new FrameworkReducer();

            foreach (var packageToInstall in packagesToInstall)
            {
                var (package, installPath) = await _nugetService.DownloadPackage(packageToInstall);
                
                var libItems = package.GetLibItems().ToList();
                var possibleFrameworks = libItems.Select(group => group.TargetFramework);
                var nearestFramework = frameworkReducer.GetNearest(nuGetFramework, possibleFrameworks);
                
                var assemblies = libItems
                    .Where(group => group.TargetFramework == nearestFramework)
                    .SelectMany(group => group.Items)
                    .ToList();
                
                foreach (var assembly in assemblies)
                {
                    var destFileName = Path.Combine(pluginInstallFolder, Path.GetFileName(assembly));

                    if (File.Exists(destFileName)) continue;

                    var sourceFileName = Path.Combine(installPath, assembly);
                    File.Copy(sourceFileName, destFileName);
                }

                var contentItems = package.GetContentItems().ToList();
                var nearestContentFramework = frameworkReducer.GetNearest(nuGetFramework, contentItems.Select(x => x.TargetFramework));

                var ci = contentItems
                    .Where(group => group.TargetFramework == nearestContentFramework)
                    .SelectMany(group => group.Items)
                    .ToList();

                foreach (var contentItem in ci)
                {
                    var destFileName = Path.Combine(pluginInstallFolder,contentItem);
                    
                    var folder = Path.GetDirectoryName(destFileName);
                    if (folder!= null && !Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    
                    if (File.Exists(destFileName)) continue;

                    var sourceFileName = Path.Combine(installPath, contentItem);
                    File.Copy(sourceFileName, destFileName);
                }
            }
        }
    }
}