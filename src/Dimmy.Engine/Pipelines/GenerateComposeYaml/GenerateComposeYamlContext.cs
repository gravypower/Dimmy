using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Pipelines.GenerateComposeYaml
{
    public class GenerateComposeYamlContext : IGenerateComposeYamlContext
    {
        public string WorkingPath { get; set; }
        public ProjectInstanceYaml ProjectInstance { get; set; }
        public ProjectYaml Project { get; set; }
    }
}