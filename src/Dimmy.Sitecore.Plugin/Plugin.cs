using Dimmy.Cli;
using SimpleInjector;

namespace Dimmy.Sitecore.Plugin
{
    public class Plugin:IPlugin
    {
        public void Bootstrap(Container container)
        {
            container.Collection.Register<ITopology>(GetType().Assembly);
        }
    }
}
