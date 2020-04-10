using Octostache;

namespace DIMS.Engine
{
    public interface ITopology
    {
        string DockerComposeTemplate { get; }

        VariableDictionary VariableDictionary { get; }
    }
}
