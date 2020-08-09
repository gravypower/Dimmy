using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Engine.Models;
using Dimmy.Engine.Services;
using Dimmy.Engine.Services.Projects;
using NuGet.Protocol.Core.Types;

namespace Dimmy.Engine.Queries.Projects
{
    public class GetRunningProjectsQueryHandler : IQueryHandler<GetRunningProjects, IList<Project>>
    {
        private readonly IProjectService _projectService;

        public GetRunningProjectsQueryHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public IList<Project> Handle(GetRunningProjects query)
        {
            return _projectService.RunningProjects().ToList();
        }
    }
}