using System.CommandLine;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Project;

namespace Dimmy.Cli.Commands.Project
{
    public abstract class InitialiseSubCommand
    {
        protected readonly ICommandHandler<InitialiseProject> InitialiseProjectCommandHandler;

        protected InitialiseSubCommand(ICommandHandler<InitialiseProject> initialiseProjectCommandHandler)
        {
            InitialiseProjectCommandHandler = initialiseProjectCommandHandler;
        }

        protected abstract string Name { get; }
        protected abstract string Description { get; }

        public Command GetCommand()
        {
            var command = new Command(Name, Description)
            {
                new Option<string>("--name"),
                new Option<string>("--source-code-path"),
                new Option<string>("--project-path"),
                new Option<string>("--docker-compose-template"),
            };

            HydrateCommand(command);

            return command;
        }


        protected abstract void HydrateCommand(Command command);

    }
}