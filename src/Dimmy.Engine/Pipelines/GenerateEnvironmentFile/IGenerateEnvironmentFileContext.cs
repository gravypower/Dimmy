using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Pipelines.GenerateEnvironmentFile
{
    public interface IGenerateEnvironmentFileContext
    {
        string WorkingPath { get; set; }
        ProjectInstanceYaml ProjectInstance { get; set; }
        ProjectYaml Project { get; set; }
    }
}