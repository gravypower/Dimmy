using System.IO;

namespace Dimmy.Engine.Commands.Docker
{
    public class GenerateComposeYaml :ICommand
    {
        public Stream LicenseStream { get; set; }
        public string ProjectFolder { get; set; }
        public string ProjectName { get; set; }
        public ITopology Topology { get; set; }
        public string SqlSaPassword { get; set; }
        public string TelerikEncryptionKey { get; set; }
        public string SourcePath { get; set; }
    }
}
