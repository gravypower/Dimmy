using Dimmy.Engine.Commands.Project;

namespace Dimmy.Sitecore.Plugin
{
    public class SitecoreInitialise : InitialiseProject
    {

        public string Registry{ get; set; }
        public string SitecoreVersion { get; set; } = "9.3.0";
        public string NanoServerVersion { get; set; } = "1809";
        public string WindowsServerCoreVersion { get; set; } = "ltsc2019";
        public string LicensePath{ get; set; }
        public string TopologyName{ get; set; }
    }
}