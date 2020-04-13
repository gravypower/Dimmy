using System;
using System.IO;

namespace DIMS.Engine.Commands.Docker
{
    public class GenerateComposeYaml :ICommand
    {
        public Stream LicenseStream { get; set; }
        public string ProjectFolder { get; set; }
        public string ProjectName { get; set; }
        public ITopology Topology { get; set; }
        public string SqlSaPassword { get; set; } = "P@ssw0rd!123";
        public string TelerikEncryptionKey { get; set; } = $"{Guid.NewGuid():N}";
    }
}
