using System.Collections.Generic;
using Dimmy.Engine.Services;

namespace Dimmy.Engine.Pipelines.InstallPlugin
{
    public class InstallPluginPipeline: Pipeline<Node<IInstallPluginContext>, IInstallPluginContext>
    {
        public InstallPluginPipeline(
            INugetService nugetService,
            IEnumerable<Node<IInstallPluginContext>> nodes) : base(nodes)
        {
        }
    }
}