using SimpleInjector;

namespace Dimmy.Cli.Plugins
{
    public interface IPlugin
    {
        void Bootstrap(Container container);
    }
}
