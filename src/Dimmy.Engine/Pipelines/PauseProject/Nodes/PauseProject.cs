using System;
using System.Linq;
using Dimmy.Engine.Pipelines.StopProject;
using Dimmy.Engine.Services.Projects;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Pipelines.PauseProject.Nodes
{
    public class PauseProject : Node<IPauseProjectContext>
    {
        private readonly IHostService _hostService;
        private readonly IProjectService _projectService;

        public PauseProject(
            IHostService hostService,
            IProjectService projectService)
        {
            _hostService = hostService;
            _projectService = projectService;
        }
        public override void DoExecute(IPauseProjectContext input)
        {
            var project = _projectService.GetProjectById(input.ProjectId);

            foreach (var c in _hostService.GetContainers())
            {
                if (project.Services.All(r => r.ContainerId != c.Id))
                    continue;

                if (c.State != ServiceRunningState.Running && c.State != ServiceRunningState.Starting &&
                    c.State != ServiceRunningState.Paused)
                    continue;
                
                Console.WriteLine($"Stopping {c.Name}");
                
                c.Pause();
            }
        }
    }
}