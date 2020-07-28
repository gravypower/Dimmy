using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;
using Dimmy.Engine.Services;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Stop:IProjectSubCommand
    {
        private readonly IProjectService _projectService;
        private readonly ICommandHandler<StopProject> _stopProjectCommandHandler;

        public Stop(
            IProjectService projectService,
            ICommandHandler<StopProject> stopProjectCommandHandler)
        {
            _projectService = projectService;
            _stopProjectCommandHandler = stopProjectCommandHandler;
        }

        public Command GetCommand()
        {
            var stopProjectCommand = new Command("stop")
            {
                new Option<Guid>("--project-id", "Project Id")
            };

            stopProjectCommand.Handler = CommandHandler.Create(async (StopArgument arg) =>
                {
                    if (arg.ProjectId == Guid.Empty)
                    {
                        var (projectInstance, project) = _projectService.GetProject();
                        arg.ProjectId = projectInstance.Id;
                    }

                    var stopProject = new StopProject
                    {
                        ProjectId = arg.ProjectId
                    };

                    await _stopProjectCommandHandler.Handle(stopProject);
                });
            return stopProjectCommand;
        }
    }
}