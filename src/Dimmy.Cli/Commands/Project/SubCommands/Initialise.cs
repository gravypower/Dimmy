using System.Collections.Generic;
using System.CommandLine;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Initialise : ProjectSubCommand<InitialiseArgument>
    {
        private readonly IEnumerable<InitialiseSubCommand> _initialiseSubCommands;

        public Initialise(IEnumerable<InitialiseSubCommand> initialiseSubCommands)
        {
            _initialiseSubCommands = initialiseSubCommands;
        }

        public override Command GetCommand()
        {
            var command = new Command("initialise", "Initialise a project");
            command.AddAlias("init");

            foreach (var initialiseSubCommand in _initialiseSubCommands)
                command.AddCommand(initialiseSubCommand.GetCommand());

            return command;
        }

        public  override void CommandAction(InitialiseArgument arg)
        {
            
        }
    }
}