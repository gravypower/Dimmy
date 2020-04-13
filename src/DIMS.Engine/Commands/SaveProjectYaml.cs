using DIMS.Engine.Models;

namespace DIMS.Engine.Commands
{
    public class SaveProjectYaml:ICommand
    {
        public string SavePath { get; set; }
        public ProjectYaml ProjectYaml { get; set; }
    }
}
