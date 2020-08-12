using System;
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
        
        public override Command GetCommand()
        {
            var command = new Command("project");

            foreach (var projectSubCommand in _projectSubCommands)
            {
                var c = projectSubCommand.GetCommand();
                
                var methods = projectSubCommand.GetType()
                    .GetMethods()
                    .Where(x => x.Name == "CommandAction");
                var methodInfo = methods.First();
                var actionT = typeof(Action<>).MakeGenericType(projectSubCommand.ArgumentType);
                var d =  Delegate.CreateDelegate(actionT, projectSubCommand, methodInfo);
                c.Handler = HandlerDescriptor.FromDelegate(d).GetCommandHandler();
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