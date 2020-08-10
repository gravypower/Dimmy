using System.CommandLine;

namespace Dimmy.Cli.Commands
{
    public interface ICommand
    {
        void CommandAction(object arg);
        Command GetCommand();
    }
}