using System;
using System.CommandLine;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;
using Dimmy.Engine.Pipelines;
using Dimmy.Engine.Pipelines.PauseProject;
using Dimmy.Engine.Pipelines.PauseProject.Nodes;
using Dimmy.Engine.Services.Projects;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Pause : ProjectSubCommand<PauseArgument>
    {
        private readonly IProjectService _projectService;
        private readonly Pipeline<Node<IPauseProjectContext>, IPauseProjectContext> _pauseProjectPipeline;

        public Pause(
            IProjectService projectService,
            Pipeline<Node<IPauseProjectContext>, IPauseProjectContext> pauseProjectPipeline)
        {
            _projectService = projectService;
            _pauseProjectPipeline = pauseProjectPipeline;
        }

        public override Command BuildCommand()
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
            
            _pauseProjectPipeline.Execute(new PauseProjectContext
            {
                ProjectId = arg.ProjectId
            });
            
        }
    }
}