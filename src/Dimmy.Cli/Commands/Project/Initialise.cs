using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dimmy.Cli.Commands.Project
{
    public class Initialise: IProjectSubCommand
    {
        private readonly IEnumerable<InitialiseSubCommand> _initialiseSubCommands;

        public Initialise(IEnumerable<InitialiseSubCommand> initialiseSubCommands)
        {
            _initialiseSubCommands = initialiseSubCommands;
        }

        public Command GetCommand()
        {
            var command = new Command("initialise", "Initialise a project");
            command.AddAlias("init");

            foreach (var initialiseSubCommand in _initialiseSubCommands)
            {
                command.AddCommand(initialiseSubCommand.GetCommand());
            }

            command.Handler = CommandHandler
                .Create<string, string>((name, plugin) =>
                {

                });

            return command;
        }
    }
}
