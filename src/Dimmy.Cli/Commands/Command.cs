using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Linq;

namespace Dimmy.Cli.Commands
{
    public abstract class Command<TCommandArgument>: ICommand
    where TCommandArgument : CommandArgument
    {
        public virtual void CommandAction(TCommandArgument arg)
        {
        }

        public abstract Command BuildCommand();
        
        protected void AddSubCommands(Command command, IEnumerable<ICommand> subCommands)
        {
            foreach (var projectSubCommand in subCommands)
            {
                var c = projectSubCommand.BuildCommand();

                var methods = projectSubCommand.GetType()
                    .GetMethods()
                    .Where(x => x.Name == nameof(CommandAction));
                var methodInfo = methods.First();
                c.Handler = HandlerDescriptor.FromMethodInfo(methodInfo, projectSubCommand).GetCommandHandler();
                command.AddCommand(c);
            }
        }
    }
}