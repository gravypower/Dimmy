using System.Collections.Generic;

namespace Dimmy.Engine.Pipelines.GenerateComposeYaml
{
    public class GenerateComposeYamlPipeline: Pipeline<Node<IGenerateComposeYamlContext>, IGenerateComposeYamlContext>
    {
        public GenerateComposeYamlPipeline(IEnumerable<Node<IGenerateComposeYamlContext>> nodes) : base(nodes)
        {
        }
    }
}