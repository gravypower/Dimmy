using System.CommandLine;

namespace Dimmy.Cli.RootCommands
{
    public interface ICommandLineCommand
    {
        Command GetCommand();
    }
}
