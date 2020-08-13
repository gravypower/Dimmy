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

            foreach (var projectSubCommand in _projectSubCommands)
            {
                var c = projectSubCommand.BuildCommand();
                
                var methods = projectSubCommand.GetType()
                    .GetMethods()
                    .Where(x => x.Name == nameof(CommandAction));
                var methodInfo = methods.First();
                c.Handler = HandlerDescriptor.FromMethodInfo(methodInfo, projectSubCommand).GetCommandHandler();
                command.AddCommand(c);
            }

            command.Handler = CommandHandler.Create((ProjectArgument arg) => CommandAction(arg));
            
            return command;
        }

        public  override void CommandAction(ProjectArgument arg)
        {
        }
    }
}