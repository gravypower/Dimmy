using System.Collections.Generic;

namespace Dimmy.Engine.Pipelines.CopyFileToContainer
{
    public class CopyFileToContainerPipeline: Pipeline<Node<ICopyFileToContainerContext>, ICopyFileToContainerContext>
    {
        public CopyFileToContainerPipeline(IEnumerable<Node<ICopyFileToContainerContext>> nodes) : base(nodes)
        {
        }
    }
}