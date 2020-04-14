using Octostache;

namespace Dimmy.Engine
{
    public interface ITopology
    {
        string DockerComposeTemplate { get; }

        VariableDictionary VariableDictionary { get; }
    }
}
