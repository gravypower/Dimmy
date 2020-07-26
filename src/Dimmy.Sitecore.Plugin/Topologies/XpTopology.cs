namespace Dimmy.Sitecore.Plugin.Topologies
{
    public class XpTopology:Topology
    {
        protected override string DockerComposeTemplateManifestResourceName { get; } = "Dimmy.Sitecore.Plugin.TopologyTemplates.docker-compose.xp.yml.template";
        public override string Name => "xp";
        public override string DockerComposeTemplateName => "docker-compose.xp.yml.template";
    }
}
