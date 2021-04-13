using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Pipelines.GenerateComposeYaml
{
    public interface IGenerateComposeYamlContext
    {
        string WorkingPath { get; set; }
        ProjectInstanceYaml ProjectInstance { get; set; }
        ProjectYaml Project { get; set; }
    }
}