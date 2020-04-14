using System.CommandLine;
using System.CommandLine.Invocation;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;

namespace Dimmy.Cli.RootCommands.Project
{
    public class StartProjectSubCommand : IProjectSubCommand
    {
        private readonly ICommandHandler<StartProject> _startProjectCommandHandler;

        public StartProjectSubCommand(ICommandHandler<StartProject> startProjectCommandHandler)
        {
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
                    if (string.IsNullOrEmpty(projectFolder))
                    {
                        projectFolder = ".";
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
