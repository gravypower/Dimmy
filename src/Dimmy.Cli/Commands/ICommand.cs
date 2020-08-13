using System.CommandLine;

namespace Dimmy.Cli.Commands
{
    public interface ICommand
    {
        Command BuildCommand();
    }
}