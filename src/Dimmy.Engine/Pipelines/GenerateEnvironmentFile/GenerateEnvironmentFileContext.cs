using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Pipelines.GenerateEnvironmentFile
{
    public class GenerateEnvironmentFileContext : IGenerateEnvironmentFileContext
    {
        public string WorkingPath { get; set; }
        public ProjectInstanceYaml ProjectInstance { get; set; }
        public ProjectYaml Project { get; set; }
    }
}