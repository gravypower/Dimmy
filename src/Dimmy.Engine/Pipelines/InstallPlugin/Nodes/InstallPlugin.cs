using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Engine.Services.Nuget;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace Dimmy.Engine.Pipelines.InstallPlugin.Nodes
{
    public class InstallPlugin: Node<IInstallPluginContext>
    {
        public override int Order => -1;
        
        private readonly INugetService _nugetService;

        public InstallPlugin(INugetService nugetService)
        {
            _nugetService = nugetService;
        }

        public override void DoExecute(IInstallPluginContext input)
        {
            var pluginInstallFolder = Path.Combine(input.InstallDirectory, input.PackageId);

            if (Directory.Exists(pluginInstallFolder))
                return;
            
            Directory.CreateDirectory(pluginInstallFolder);

            var nuGetVersion = NuGetVersion.Parse(input.PackageVersion);
            var packageIdentity = new PackageIdentity(input.PackageId, nuGetVersion);
            var nuGetFramework = NuGetFramework.ParseFolder(input.PackageFramework);

            var packagesToInstall = _nugetService
                .GetPackagesToInstall(packageIdentity, nuGetFramework, input.OmitDependencies)
                .Result;

            var frameworkReducer = new FrameworkReducer();

            foreach (var packageToInstall in packagesToInstall)
            {
                var (package, installPath) = _nugetService.DownloadPackage(packageToInstall).Result;

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
                var nearestContentFramework =
                    frameworkReducer.GetNearest(nuGetFramework, contentItems.Select(x => x.TargetFramework));

                var ci = contentItems
                    .Where(group => group.TargetFramework == nearestContentFramework)
                    .SelectMany(group => group.Items)
                    .ToList();

                foreach (var contentItem in ci)
                {
                    var destFileName = Path.Combine(pluginInstallFolder, contentItem);

                    var folder = Path.GetDirectoryName(destFileName);
                    if (folder != null && !Directory.Exists(folder))
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