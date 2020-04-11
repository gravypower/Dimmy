using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DIMS.Engine.Commands.DockerCompose
{
    public class GenerateComposeYaml :ICommand
    {
        public Stream LicenseStream { get; set; }
        public string ProjectFolder { get; set; }
        public string Project { get; set; }
        public ITopology Topology { get; set; }
        public string SqlSaPassword { get; set; } = "P@ssw0rd!123";

        public string TelerikEncryptionKey { get; set; } = $"{Guid.NewGuid():N}";


    }
}
