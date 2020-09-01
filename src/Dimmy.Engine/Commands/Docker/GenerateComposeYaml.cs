using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Commands.Docker
{
    public class GenerateComposeYaml : ICommand
    {
        public string WorkingPath { get; set; }
        public ProjectInstanceYaml ProjectInstance { get; set; }
        public ProjectYaml Project { get; set; }
    }
}