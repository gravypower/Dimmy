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
                new Option<string>("--working-folder", "Working Folder")
            };

            startProjectCommand.Handler = CommandHandler
                .Create<string>(async workingFolder =>
                {

                    if (string.IsNullOrEmpty(workingFolder))
                    {
                        workingFolder = ".";
                    }

                    if (!File.Exists(Path.Combine(workingFolder, "docker-compose.yml")))
                    {
                        await _generateComposeYamlCommandHandler.Handle(new GenerateComposeYaml
                        {
                            WorkingPath = workingFolder
                        });
                    }

                    var startProject = new StartProject
                    {
                        ProjectFolder = workingFolder
                    };

                    await _startProjectCommandHandler.Handle(startProject);
                });

            return startProjectCommand;

        }
    }
}