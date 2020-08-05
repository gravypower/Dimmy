using System.CommandLine;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public interface IInitialiseSubCommand
    {
        string Name { get; }
        string Description { get; }
        void HydrateCommand(Command command);
    }
}