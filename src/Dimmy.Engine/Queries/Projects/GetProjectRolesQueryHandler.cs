using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Engine.Models;
using Dimmy.Engine.Services;

namespace Dimmy.Engine.Queries.Projects
{
    public class GetProjectRolesQueryHandler : IQueryHandler<GetProjectRoles, IEnumerable<Service>>
    {
        private readonly IProjectService _projectService;

        public GetProjectRolesQueryHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IEnumerable<Service>> Handle(GetProjectRoles query)
        {
            var project = _projectService.RunningProjects().Single(p => p.Id == query.ProjectId);

            return await Task.FromResult(project.Services);
        }
    }
}