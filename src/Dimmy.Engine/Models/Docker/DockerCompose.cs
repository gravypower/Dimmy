using System.Collections.Generic;

namespace Dimmy.Engine.Models.Docker
{
    public class DockerCompose
    {
        public string Version { get; set; }

        public IDictionary<string, Service> Services { get; set; }
    }
}
