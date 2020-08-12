using System;
using System.Linq;
using Dimmy.Engine.Services.Projects;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Commands.Docker
{
    public class PauseProjectCommandHandler : ICommandHandler<PauseProject>
    {
        private readonly IHostService _hostService;
        private readonly IProjectService _projectService;

        public PauseProjectCommandHandler(
            IHostService hostService,
            IProjectService projectService)
        {
            _hostService = hostService;
            _projectService = projectService;
        }

        public void Handle(PauseProject command)
        {
            var project = _projectService.GetProjectById(command.ProjectId);

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