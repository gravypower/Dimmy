using System.Collections.Generic;
using Dimmy.Engine.Services;
using Dimmy.Engine.Services.Nuget;

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