namespace Dimmy.Engine.Models
{
    public class ProjectInstanceYaml: ProjectYaml
    {
        public string SourceCodeLocation { get; set; }
        public string WorkingPath { get; set; }
    }
}
