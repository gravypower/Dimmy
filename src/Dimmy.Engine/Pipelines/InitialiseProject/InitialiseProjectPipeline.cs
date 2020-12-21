using System.Collections.Generic;

namespace Dimmy.Engine.Pipelines.InitialiseProject
{
    public class InitialiseProjectPipeline: Pipeline<Node<IInitialiseProjectContext>, IInitialiseProjectContext>
    {
        public InitialiseProjectPipeline(IEnumerable<Node<IInitialiseProjectContext>> nodes) : base(nodes)
        {
        }
    }
}