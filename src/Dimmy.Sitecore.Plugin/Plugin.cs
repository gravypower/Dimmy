using ClassLibrary1;
using Dimmy.Cli.Plugins;
using SimpleInjector;

namespace Dimmy.Sitecore.Plugin
{
    public class Plugin:IPlugin
    {
        public void Bootstrap(Container container)
        {
            container.Collection.Register<ITopology>(GetType().Assembly);

            new Class1().DoSomething();
        }
    }
}
