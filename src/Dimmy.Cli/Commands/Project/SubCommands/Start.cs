using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Start : IProjectSubCommand
    {
        private readonly ICommandHandler<GenerateComposeYaml> _generateComposeYamlCommandHandler;
        private readonly ICommandHandler<StartProject> _startProjectCommandHandler;

        public Start(
            ICommandHandler<GenerateComposeYaml> generateComposeYamlCommandHandler,
            ICommandHandler<StartProject> startProjectCommandHandler)
        {
            _generateComposeYamlCommandHandler = generateComposeYamlCommandHandler;
            _startProjectCommandHandler = startProjectCommandHandler;
        }

        public Command GetCommand()
        {
            var startProjectCommand = new Command("start")
            {
                new Option<string>("--working-path", "Working Path"),
                new Option<bool>("--generate-only", "Don't start the project only generate the docker compose file")
            };

            startProjectCommand.Handler = CommandHandler.Create((StartArgument arg) =>
            {
                if (string.IsNullOrEmpty(arg.WorkingPath))
                    arg.WorkingPath = Path.GetFullPath(Environment.CurrentDirectory);

                var workingDockerCompose = Path.Combine(arg.WorkingPath, "docker-compose.yml");
                if (File.Exists(workingDockerCompose)) File.Delete(workingDockerCompose);

                _generateComposeYamlCommandHandler.Handle(new GenerateComposeYaml
                {
                    WorkingPath = arg.WorkingPath
                });


                if (arg.GeneratOnly)
                    return;

                var startProject = new StartProject
                {
                    DockerComposeFilePath = Path.Combine(arg.WorkingPath, "docker-compose.yml")
                };

                _startProjectCommandHandler.Handle(startProject);
            });

            return startProjectCommand;
        }
    }
}