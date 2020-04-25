using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;

namespace Dimmy.Cli.Commands.Project
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
                new Option<string>("--working-path", "Working Path")
            };

            startProjectCommand.Handler = CommandHandler
                .Create<string>(async workingPath =>
                {

                    if (string.IsNullOrEmpty(workingPath))
                    {
                        workingPath = ".";
                    }

                    var workingDockerCompose = Path.Combine(workingPath, "docker-compose.yml");
                    if (File.Exists(workingDockerCompose))
                    {
                        File.Delete(workingDockerCompose);
                    }

                    await _generateComposeYamlCommandHandler.Handle(new GenerateComposeYaml
                    {
                        WorkingPath = workingPath
                    });

                    var startProject = new StartProject
                    {
                        DockerComposeFilePath= Path.Combine(workingPath, "docker-compose.yml")
                    };

                    await _startProjectCommandHandler.Handle(startProject);
                });

            return startProjectCommand;

        }
    }
}