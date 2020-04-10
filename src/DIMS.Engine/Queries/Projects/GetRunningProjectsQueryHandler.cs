using Ductus.FluentDocker.Services;

namespace DIMS.Engine.Queries.Projects
{
    public class GetRunningProjectsQueryHandler:IQueryHandler<GetRunningProjects, string>
    {
        private readonly IHostService _hostService;

        public GetRunningProjectsQueryHandler(IHostService hostService)
        {
            _hostService = hostService;
        }

        public string Handle(GetRunningProjects query)
        {
            var ac = _hostService.GetRunningContainers();


            return "";
        }
    }
}
