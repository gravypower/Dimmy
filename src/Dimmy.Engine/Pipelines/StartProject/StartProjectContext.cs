using Dimmy.Engine.Models.Yaml;
using Dimmy.Engine.Models.Yaml.DockerCompose;

namespace Dimmy.Engine.Pipelines.StartProject
{
    public class StartProjectContext:IStartProjectContext
    {
        public DockerComposeYaml DockerComposeYaml { get; set; }
        public ProjectInstanceYaml ProjectInstance { get; set; } 
        public ProjectYaml Project { get; set; }
        public string WorkingPath { get; set; }
        public bool GeneratOnly { get; set; }
    }
}