using System;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Engine.Services.Projects;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Pipelines.StopProject.Nodes
{
    public class StopProject : Node<IStopProjectContext>
    {
        private readonly IHostService _hostService;
        private readonly IProjectService _projectService;

        public StopProject(
            IHostService hostService,
            IProjectService projectService)
        {
            _hostService = hostService;
            _projectService = projectService;
        }
        public override async Task DoExecute(IStopProjectContext input)
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

                c.Remove(true);
            }
        }
    }
}