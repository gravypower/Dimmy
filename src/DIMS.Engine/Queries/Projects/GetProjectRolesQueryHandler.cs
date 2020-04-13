using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIMS.Engine.Models;
using DIMS.Engine.Services;

namespace DIMS.Engine.Queries.Projects
{
    public class GetProjectRolesQueryHandler:IQueryHandler<GetProjectRoles, IEnumerable<Role>>
    {
        private readonly IProjectService _projectService;

        public GetProjectRolesQueryHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IEnumerable<Role>> Handle(GetProjectRoles query)
        {
            var project = _projectService.RunningProjects().Single(p => p.Id == query.ProjectId);

            return await Task.FromResult(project.Roles);
        }
    }
}
