using System.Collections.Generic;
using System.Threading.Tasks;
using DIMS.Engine.Models;
using DIMS.Engine.Services;

namespace DIMS.Engine.Queries.Projects
{
    public class GetRunningProjectsQueryHandler:IQueryHandler<GetRunningProjects, IEnumerable<Project>>
    {
        private readonly IProjectService _projectService;

        public GetRunningProjectsQueryHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IEnumerable<Project>> Handle(GetRunningProjects query)
        {
            return await Task.FromResult(_projectService.RunningProjects());
        }
    }
}
