using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;
using Dimmy.Engine.Services;

namespace Dimmy.Cli.Commands.Project
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

            stopProjectCommand.Handler = CommandHandler
                .Create<Guid>(async projectId =>
                {
                    if (projectId == Guid.Empty)
                    {
                        var (projectInstance, project) = _projectService.GetProject();
                        projectId = projectInstance.Id;
                    }

                    var stopProject = new StopProject
                    {
                        ProjectId = projectId
                    };

                    await _stopProjectCommandHandler.Handle(stopProject);
                });
            return stopProjectCommand;
        }
    }
}