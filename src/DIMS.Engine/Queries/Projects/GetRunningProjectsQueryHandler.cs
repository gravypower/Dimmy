using System.Collections.Generic;
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

        public IEnumerable<Project> Handle(GetRunningProjects query)
        {
            return _projectService.RunningProjects();
        }
    }
}
