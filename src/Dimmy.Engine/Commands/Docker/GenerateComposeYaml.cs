using System.IO;
using Dimmy.Engine.Services;

namespace Dimmy.Engine.Commands.Docker
{
    public class GenerateComposeYaml :ICommand
    {
        public Stream LicenseStream { get; set; }
        public string ProjectFolder { get; set; }
        public string ProjectName { get; set; }
        public ITopology Topology { get; set; }
        public string SqlSaPassword { get; set; } = NonceService.Generate();
        public string TelerikEncryptionKey { get; set; } = NonceService.Generate();
    }
}
