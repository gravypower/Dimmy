using System;
using System.CommandLine;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;
using Dimmy.Engine.Services.Projects;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Pause : ProjectSubCommand<PauseArgument>
    {
        private readonly IProjectService _projectService;
        private readonly ICommandHandler<PauseProject> _stopProjectCommandHandler;

        public Pause(
            IProjectService projectService,
            ICommandHandler<PauseProject> stopProjectCommandHandler)
        {
            _projectService = projectService;
            _stopProjectCommandHandler = stopProjectCommandHandler;
        }

        public override Command GetCommand()
        {
            var stopProjectCommand = new Command("pause")
            {
                new Option<string>("--working-path", "Working Path"),
                new Option<Guid>("--project-id", "Project Id")
            };
            
            return stopProjectCommand;
        }

        public override void CommandAction(PauseArgument arg)
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
            
            var stopProject = new PauseProject
            {
                ProjectId = arg.ProjectId
            };

            _stopProjectCommandHandler.Handle(stopProject);
        }
    }
}