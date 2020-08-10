using System;
using System.CommandLine;

namespace Dimmy.Cli.Commands
{
    public abstract class Command<TCommandArgument>: ICommand
    where TCommandArgument : CommandArgument
    {
        protected abstract void CommandAction(TCommandArgument arg);
        public void CommandAction(object arg)
        {
            CommandAction(arg as TCommandArgument);
        }

        public abstract Command GetCommand();
        public Type ArgumentType => typeof(TCommandArgument);
    }
}