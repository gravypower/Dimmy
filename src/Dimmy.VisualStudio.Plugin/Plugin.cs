using Dimmy.Cli.Plugins;
using Dimmy.VisualStudio.Plugin.Services.VisualStudioRemoteTools;
using SimpleInjector;

namespace Dimmy.VisualStudio.Plugin
{
    public class Plugin:IPlugin
    {
        public void Bootstrap(Container container)
        {
            container.Collection.Register<IVisualStudioRemoteTool>(GetType().Assembly);
        }
    }
}
