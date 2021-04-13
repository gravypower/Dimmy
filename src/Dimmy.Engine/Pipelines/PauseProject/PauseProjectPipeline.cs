using System.Collections.Generic;

namespace Dimmy.Engine.Pipelines.PauseProject
{
    public class PauseProjectPipeline: Pipeline<Node<IPauseProjectContext>, IPauseProjectContext>
    {
        public PauseProjectPipeline(IEnumerable<Node<IPauseProjectContext>> nodes) : base(nodes)
        {
        }
    }
}