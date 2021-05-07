using System.Collections.Generic;

namespace Dimmy.Engine.Pipelines.GenerateDockerComposeFile
{
    public class GenerateEnvironmentFilePipeline: Pipeline<Node<IGenerateDockerComposeFileContext>, IGenerateDockerComposeFileContext>
    {
        public GenerateEnvironmentFilePipeline(IEnumerable<Node<IGenerateDockerComposeFileContext>> nodes) : base(nodes)
        {
        }
    }
}