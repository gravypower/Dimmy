using System.CommandLine;

namespace DIMS.CLI.RootCommands
{
    public interface ICommandLineCommand
    {
        Command GetCommand();
    }
}
