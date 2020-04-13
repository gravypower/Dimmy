using System.Linq;
using DIMS.Engine.Services;
using Ductus.FluentDocker.Services;

namespace DIMS.Engine.Commands.Docker
{
    public class StopProjectCommandHandler:ICommandHandler<StopProject>
    {
        private readonly IHostService _hostService;
        private readonly IProjectService _projectService;

        public StopProjectCommandHandler(
            IHostService hostService,
            IProjectService projectService)
        {
            _hostService = hostService;
            _projectService = projectService;
        }

        public void Handle(StopProject command)
        {
            var project = _projectService.GetProjectById(command.ProjectId);

            foreach (var c in _hostService.GetContainers())
            {
                if (project.Roles.All(r => r.ContainerId != c.Id)) 
                    continue;

                if (c.State != ServiceRunningState.Running && c.State != ServiceRunningState.Starting && c.State != ServiceRunningState.Paused)
                    continue;

                c.Stop();

            }
        }
    }
}
