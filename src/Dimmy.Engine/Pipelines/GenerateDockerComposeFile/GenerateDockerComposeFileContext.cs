using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Pipelines.GenerateDockerComposeFile
{
    public class GenerateDockerComposeFileContext : IGenerateDockerComposeFileContext
    {
        public string WorkingPath { get; set; }
        public ProjectInstanceYaml ProjectInstance { get; set; }
        public ProjectYaml Project { get; set; }
    }
}