using System.IO;

namespace Dimmy.Engine.Pipelines.InstallPlugin
{
    public class InstallPluginContext : IInstallPluginContext
    {
        public string PackageId { get; set; }
        public string PackageVersion { get; set; }
        public string PackageFramework { get; set; }
        public string InstallDirectory { get; set; }
        public string[] OmitDependencies { get; set; }
        public string PluginInstallFolder => Path.Combine(InstallDirectory, PackageId);
    }
}