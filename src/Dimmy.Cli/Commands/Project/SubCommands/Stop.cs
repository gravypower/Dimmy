using System;
using System.CommandLine;
using Dimmy.Engine.Pipelines;
using Dimmy.Engine.Pipelines.StopProject;
using Dimmy.Engine.Services.Projects;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Stop : ProjectSubCommand<StopArgument>
    {
        private readonly IProjectService _projectService;
        private readonly Pipeline<Node<IStopProjectContext>, IStopProjectContext> _stopProjectPipeline;


        public Stop(
            IProjectService projectService,
            Pipeline<Node<IStopProjectContext>, IStopProjectContext> stopProjectPipeline)
        {
            _projectService = projectService;
            _stopProjectPipeline = stopProjectPipeline;
        }

        public override Command BuildCommand()
        {
            var stopProjectCommand = new Command("stop")
            {
                new Option<string>("--working-path", "Working Path"),
                new Option<Guid>("--project-id", "Project Id")
            };
            
            return stopProjectCommand;
        }

        public  override void CommandAction(StopArgument arg)
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
            
            _stopProjectPipeline.Execute(new StopProjectContext
            {
                ProjectId = arg.ProjectId
            });
        }
    }
}