using System.Collections.Generic;

namespace Dimmy.Engine.Pipelines.StartProject
{
    public class StartProjectPipeline: Pipeline<Node<IStartProjectContext>, IStartProjectContext>
    {
        public StartProjectPipeline(IEnumerable<Node<IStartProjectContext>> nodes) : base(nodes)
        {
        }
    }
}