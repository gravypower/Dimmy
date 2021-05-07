using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Pipelines.GenerateDockerComposeFile
{
    public interface IGenerateDockerComposeFileContext
    {
        string WorkingPath { get; set; }
        ProjectInstanceYaml ProjectInstance { get; set; }
        ProjectYaml Project { get; set; }
    }
}