using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.Linq;

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

            command.Handler = CommandHandler.Create((ProjectArgument arg) => CommandAction(arg));
            
            return command;
        }

        

        public  override void CommandAction(ProjectArgument arg)
        {
        }
    }
}