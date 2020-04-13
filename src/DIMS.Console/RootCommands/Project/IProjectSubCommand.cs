using System.CommandLine;

namespace DIMS.CLI.RootCommands.Project
{
    interface IProjectSubCommand
    {
        Command GetCommand();
    }
}
