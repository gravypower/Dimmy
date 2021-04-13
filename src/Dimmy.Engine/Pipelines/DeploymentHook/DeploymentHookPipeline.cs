using System.Collections.Generic;

namespace Dimmy.Engine.Pipelines.DeploymentHook
{
    public class DeploymentHookPipeline: Pipeline<Node<IDeploymentHookContext>, IDeploymentHookContext>
    {
        public DeploymentHookPipeline(IEnumerable<Node<IDeploymentHookContext>> nodes) : base(nodes)
        {
        }
    }
}