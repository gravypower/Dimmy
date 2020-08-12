using System;
using System.CommandLine;

namespace Dimmy.Cli.Commands
{
    public abstract class Command<TCommandArgument>: ICommand
    where TCommandArgument : CommandArgument
    {
        public abstract void CommandAction(TCommandArgument arg);

        public abstract Command GetCommand();
        public Type ArgumentType => typeof(TCommandArgument);
    }
}