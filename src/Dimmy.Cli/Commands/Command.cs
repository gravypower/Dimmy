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

                var projectSubCommandType = projectSubCommand.GetType();
                var methods = projectSubCommandType
                    .GetMethods()
                    .Where(x => x.Name == nameof(CommandAction));
                
                var methodInfo = methods.First();
                
                if (methodInfo.DeclaringType == projectSubCommandType)
                {
                    c.Handler = HandlerDescriptor.FromMethodInfo(methodInfo, projectSubCommand).GetCommandHandler();
                }
                
                command.AddCommand(c);
            }
        }
    }
}