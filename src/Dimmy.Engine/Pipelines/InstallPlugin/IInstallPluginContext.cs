namespace Dimmy.Engine.Pipelines.InstallPlugin
{
    public interface IInstallPluginContext
    {
        string PackageId { get; set; }
        string PackageVersion { get; set; }
        string PackageFramework { get; set; }
        string InstallDirectory { get; set; }
        string[] OmitDependencies { get; set; }
        
        string PluginInstallFolder { get; }
    }
}