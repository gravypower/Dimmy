using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Commands.Project
{
    public class SaveProjectYaml : ICommand
    {
        public string SavePath { get; set; }
        public ProjectYaml ProjectYaml { get; set; }
    }
}