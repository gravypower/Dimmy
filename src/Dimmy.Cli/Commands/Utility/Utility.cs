using System.CommandLine;

namespace Dimmy.Cli.Commands.Utility
{
    public class Utility:ICommand
    {
        public Command BuildCommand()
        {
            var utilCommand = new Command("utility");
            return utilCommand;
        }
    }
}