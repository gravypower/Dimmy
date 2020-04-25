namespace Dimmy.Engine.Commands.Plugins
{
    public class InstallPlugin:ICommand
    {
        public string PackageId { get; set; }
        public string PackageVersion { get; set; }
        public string InstallDirectory { get; set; }
        public string[] OmitDependencies { get; set; }

    }
}
