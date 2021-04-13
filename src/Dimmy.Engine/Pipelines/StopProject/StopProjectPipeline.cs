using System.Collections.Generic;

namespace Dimmy.Engine.Pipelines.StopProject
{
    public class StopProjectPipeline: Pipeline<Node<IStopProjectContext>, IStopProjectContext>
    {
        public StopProjectPipeline(IEnumerable<Node<IStopProjectContext>> nodes) : base(nodes)
        {
        }
    }
}