using System.Collections.Generic;

namespace Dimmy.Engine.Models.Yaml.DockerCompose
{
    public class DockerComposeYaml
    {
        public string Version { get; set; }

        public IDictionary<string, Service> Services { get; set; }
    }
}
