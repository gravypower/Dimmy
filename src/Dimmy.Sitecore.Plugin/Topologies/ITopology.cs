using Octostache;

namespace Dimmy.Sitecore.Plugin.Topologies
{
    public interface ITopology
    {
        string Name { get; }
        string DockerComposeTemplate { get; }
        string DockerComposeTemplateName { get; }
        VariableDictionary VariableDictionary { get; }
    }
}
