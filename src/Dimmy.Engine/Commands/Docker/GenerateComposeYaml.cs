using System.IO;

namespace Dimmy.Engine.Commands.Docker
{
    public class GenerateComposeYaml :ICommand
    {
        public Stream LicenseStream { get; set; }
        public string ProjectFolder { get; set; }
        public string ProjectName { get; set; }
        string DockerComposeTemplate { get; }
        public string SourcePath { get; set; }
    }
}
