using System.CommandLine;

namespace Dimmy.Cli.Commands.Project
{
    public interface IProjectSubCommand
    {
        Command GetCommand();
    }
}