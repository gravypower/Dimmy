using System.CommandLine;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Project;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public abstract class InitialiseSubCommand : IInitialiseSubCommand
    {
        protected readonly ICommandHandler<InitialiseProject> InitialiseProjectCommandHandler;

        protected InitialiseSubCommand(ICommandHandler<InitialiseProject> initialiseProjectCommandHandler)
        {
            InitialiseProjectCommandHandler = initialiseProjectCommandHandler;
        }

        public abstract string Name { get; }
        public abstract string Description { get; }

        public Command GetCommand()
        {
            var command = new Command(Name, Description)
            {
                new Option<string>("--name"),
                new Option<string>("--source-code-path"),
                new Option<string>("--working-path"),
                new Option<string>("--docker-compose-template")
            };

            HydrateCommand(command);

            return command;
        }

        public abstract void HydrateCommand(Command command);
    }
}