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
                new Option<string>("--project-folder", "Project Folder")
            };

            startProjectCommand.Handler = CommandHandler
                .Create<string>(async projectFolder =>
                {
                    if (!File.Exists(Path.Combine(projectFolder, "docker-compose.yml")))
                    {
                        await _generateComposeYamlCommandHandler.Handle(new GenerateComposeYaml
                        {
                            ProjectFolder = projectFolder
                        });
                    }

                    var startProject = new StartProject
                    {
                        ProjectFolder = projectFolder
                    };

                    await _startProjectCommandHandler.Handle(startProject);
                });

            return startProjectCommand;

        }
    }
}