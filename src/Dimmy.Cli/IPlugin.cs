using SimpleInjector;

namespace Dimmy.Cli
{
    public interface IPlugin
    {
        void Bootstrap(Container container);
    }
}
