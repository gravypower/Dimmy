using System;
using System.CommandLine;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;
using Dimmy.Engine.Services.Projects;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Stop : ProjectSubCommand<StopArgument>
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

        public override Command GetCommand()
        {
            var stopProjectCommand = new Command("stop")
            {
                new Option<string>("--working-path", "Working Path"),
                new Option<Guid>("--project-id", "Project Id")
            };
            
            return stopProjectCommand;
        }

        protected override void CommandAction(StopArgument arg)
        {
            if (arg.ProjectId == Guid.Empty && string.IsNullOrEmpty(arg.WorkingPath))
            {
                var (projectInstance, project) = _projectService.GetProject();
                arg.ProjectId = projectInstance.Id;
            }
            else if (!string.IsNullOrEmpty(arg.WorkingPath))
            {
                var (projectInstance, project) = _projectService.GetProject(arg.WorkingPath);
                arg.ProjectId = projectInstance.Id;
            }
            
            var stopProject = new StopProject
            {
                ProjectId = arg.ProjectId
            };

            _stopProjectCommandHandler.Handle(stopProject);
        }
    }
}