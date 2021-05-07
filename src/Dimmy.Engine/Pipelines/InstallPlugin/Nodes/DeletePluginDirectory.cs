using System.IO;
using System.Threading.Tasks;

namespace Dimmy.Engine.Pipelines.InstallPlugin.Nodes
{
    public class DeletePluginDirectory: Node<IInstallPluginContext>
    {
        public override int Order => -1;

        public override void DoExecute(IInstallPluginContext input)
        {
            if (Directory.Exists(input.PluginInstallFolder))
                Directory.Delete(input.PluginInstallFolder);
        }
    }
}