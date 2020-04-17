using System.CommandLine;

namespace Dimmy.Cli.Commands
{
    public interface ICommandLineCommand
    {
        Command GetCommand();
    }
}
