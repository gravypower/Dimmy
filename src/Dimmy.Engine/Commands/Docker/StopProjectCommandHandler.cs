using System.Linq;
using System.Threading.Tasks;
using Dimmy.Engine.Services;
using Dimmy.Engine.Services.Projects;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Commands.Docker
{
    public class StopProjectCommandHandler : ICommandHandler<StopProject>
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

        public Task Handle(StopProject command)
        {
            return Task.Run(() => Run(command));
        }

        private void Run(StopProject command)
        {
            var project = _projectService.GetProjectById(command.ProjectId);

            foreach (var c in _hostService.GetContainers())
            {
                if (project.Services.All(r => r.ContainerId != c.Id))
                    continue;

                if (c.State != ServiceRunningState.Running && c.State != ServiceRunningState.Starting &&
                    c.State != ServiceRunningState.Paused)
                    continue;

                c.Remove(true);
            }
        }
    }
}