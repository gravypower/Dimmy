using System.CommandLine;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public abstract class InitialiseSubCommand : IInitialiseSubCommand
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public Command GetCommand()
        {
            var command = new Command(Name, Description);

            command.AddOption(new Option<string>("--name"));
            command.AddOption(new Option<string>("--source-code-path"));
            command.AddOption(new Option<string>("--working-path"));
            command.AddOption(new Option<string>("--docker-compose-template"));
            
            HydrateCommand(command);

            return command;
        }
        public abstract void HydrateCommand(Command command);
    }
}