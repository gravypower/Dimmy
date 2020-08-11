using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Pipelines.StartProject
{
    public class StartProjectContext:IStartProjectContext
    {
        public ProjectInstanceYaml ProjectInstance { get; set; } 
        public ProjectYaml Project { get; set; }
        public bool GeneratOnly { get; set; }
    }
}