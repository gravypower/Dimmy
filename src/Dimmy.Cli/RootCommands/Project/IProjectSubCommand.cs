using System.CommandLine;

namespace Dimmy.Cli.RootCommands.Project
{
    interface IProjectSubCommand
    {
        Command GetCommand();
    }
}