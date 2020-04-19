using System;
using System.IO;
using System.Reflection;
using Octostache;

namespace Dimmy.Sitecore.Plugin
{
    public class XpTopology:ITopology
    {
        private const string DockerComposeTemplateManifestResourceName = "Dimmy.Sitecore.Plugin.TopologyTemplates.docker-compose.xp.yml.template";

        public string Name => "XP";

        public string DockerComposeTemplateName => "docker-compose.xp.yml.template";
        public string DockerComposeTemplate { get; }
        
        public VariableDictionary VariableDictionary { get; } = new VariableDictionary();

        public XpTopology()
        {
            using var stream = GetType().Assembly.GetManifestResourceStream(DockerComposeTemplateManifestResourceName);

            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream.");
            }
            using var reader = new StreamReader(stream);

            DockerComposeTemplate = reader.ReadToEnd();
        }
    }
}
