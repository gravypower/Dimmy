using System.Collections.Generic;
using System.CommandLine;

namespace Dimmy.Cli.Commands.Project
{
    public class Project : Command<ProjectArgument>
    {
        private readonly IEnumerable<IProjectSubCommand> _projectSubCommands;

        public Project(IEnumerable<IProjectSubCommand> projectSubCommands)
        {
            _projectSubCommands = projectSubCommands;
        }
        
        public override Command BuildCommand()
        {
            var command = new Command("project");

            AddSubCommands(command, _projectSubCommands);
            
            return command;
        }
    }
}