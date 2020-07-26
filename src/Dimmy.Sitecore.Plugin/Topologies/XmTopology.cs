using System;
using System.IO;
using Octostache;

namespace Dimmy.Sitecore.Plugin.Topologies
{
    public class XmTopology:Topology
    {
        protected override string DockerComposeTemplateManifestResourceName { get; } = "Dimmy.Sitecore.Plugin.TopologyTemplates.docker-compose.xm.yml.template";
        public override string Name => "xm";

        public override string DockerComposeTemplateName => "docker-compose.xm.yml.template";


    }
}
