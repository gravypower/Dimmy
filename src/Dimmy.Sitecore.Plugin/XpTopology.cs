using System;
using System.IO;
using Octostache;

namespace Dimmy.Sitecore.Plugin
{
    public class XpTopology:ITopology
    {

        public string Name => "XP";

        public string DockerComposeTemplateName => "docker-compose.xp.yml.template";
        public string DockerComposeTemplate { get; }
        
        public VariableDictionary VariableDictionary { get; } = new VariableDictionary();


        public XpTopology()
        {
            using var stream = new MemoryStream(Properties.Resources.docker_compose_xp_yml);
            
            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream.");
            }
            using var reader = new StreamReader(stream);

            DockerComposeTemplate = reader.ReadToEnd();
        }
    }
}
