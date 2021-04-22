using System.Collections.Generic;

namespace Dimmy.Engine.Pipelines.GenerateEnvironmentFile
{
    public class GenerateEnvironmentFilePipeline: Pipeline<Node<IGenerateEnvironmentFileContext>, IGenerateEnvironmentFileContext>
    {
        public GenerateEnvironmentFilePipeline(IEnumerable<Node<IGenerateEnvironmentFileContext>> nodes) : base(nodes)
        {
        }
    }
}