using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;

namespace Dimmy.Engine.Services
{
    public interface INugetService
    {
        Task<IList<IPackageSearchMetadata>> GetNugetPackagesFromTag(string tag);
        Task<IList<IPackageSearchMetadata>> GetNugetPackagesFromId(string packageId);

        Task<IEnumerable<SourcePackageDependencyInfo>> GetPackageAndDependencies(
            PackageIdentity packageId,
            NuGetFramework packageFramework,
            string[] omitDependencies);

        Task<IEnumerable<SourcePackageDependencyInfo>> GetPackagesToInstall(
            PackageIdentity packageId,
            NuGetFramework packageFramework,
            string[] omitDependencies);

        Task<(PackageReaderBase package, string installPath)> DownloadPackage(
            SourcePackageDependencyInfo packageToInstall);
    }
}