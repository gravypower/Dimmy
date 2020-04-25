using ClassLibrary1;
using Dimmy.Cli;
using Dimmy.Cli.Commands.Project;
using SimpleInjector;

namespace Dimmy.Sitecore.Plugin
{
    public class Plugin:IPlugin
    {
        public void Bootstrap(Container container)
        {
            container.Collection.Register<ITopology>(GetType().Assembly);

            container.Collection.Append<InitialiseSubCommand, InitialiseSitecore>();

            new Class1().DoSomething();
        }
    }
}
