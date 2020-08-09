using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Engine.Models;
using Dimmy.Engine.Services.Projects;
using NuGet.Protocol.Core.Types;

namespace Dimmy.Engine.Queries.Projects
{
    public class GetProjectRolesQueryHandler : IQueryHandler<GetProjectRoles, IList<Service>>
    {
        private readonly IProjectService _projectService;

        public GetProjectRolesQueryHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public IList<Service> Handle(GetProjectRoles query)
        {
            var project = _projectService.RunningProjects().Single(p => p.Id == query.ProjectId);

            return project.Services.ToList();
        }
    }
}