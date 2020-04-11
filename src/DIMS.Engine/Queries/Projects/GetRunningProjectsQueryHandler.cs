using System.Collections.Generic;
using Ductus.FluentDocker.Services;

namespace DIMS.Engine.Queries.Projects
{
    public class GetRunningProjectsQueryHandler:IQueryHandler<GetRunningProjects, IEnumerable<string>>
    {
        private readonly IHostService _hostService;

        public GetRunningProjectsQueryHandler(IHostService hostService)
        {
            _hostService = hostService;
        }

        public IEnumerable<string> Handle(GetRunningProjects query)
        {
            var containers = _hostService.GetRunningContainers();

            var projects = new List<string>();
            foreach (var container in containers)
            {
                var labels = container.GetConfiguration().Config.Labels;

                if (!labels.ContainsKey("dev.dims.project")) 
                    continue;

                var project = labels["dev.dims.project"];
                if(projects.Contains(project))
                    continue;

                projects.Add(project);
                yield return project;
            }
        }
    }
}
