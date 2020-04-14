using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dimmy.Engine.Models;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IHostService _hostService;

        public ProjectService(IHostService hostService)
        {
            _hostService = hostService;
        }

        public IEnumerable<Project> RunningProjects()
        {
            var containers = _hostService.GetRunningContainers();

            var projects = new Dictionary<Guid, Project>();

            foreach (var container in containers)
            {
                var labels = container.GetConfiguration().Config.Labels;

                if (!labels.ContainsKey(DimmyDockerComposeLabels.Project))
                    continue;

                if (!labels.ContainsKey(DimmyDockerComposeLabels.ProjectId))
                    throw new ServiceDoesNotHaveDimmyProjectIdLabel();

                var projectId = Guid.Parse(labels[DimmyDockerComposeLabels.ProjectId]);

                var projectName = labels[DimmyDockerComposeLabels.ProjectName];

                if (!projects.ContainsKey(projectId))
                {
                    projects.Add(projectId, new Project
                    {
                        Name = projectName,
                        Id = projectId
                    });
                }

                projects[projectId].Roles.Add(new Role
                {
                    Name = labels[DimmyDockerComposeLabels.ProjectRole],
                    ContainerId = container.Id
                });
            }

            return projects.Values;
        }

        public Project GetProjectById(Guid projectId)
        {
            return RunningProjects().Single(p => p.Id == projectId);
        }

        public Guid GetContextProjectId()
        {
            var projectContextFile = ".dimmy";
            if (!File.Exists(projectContextFile))
            {
                throw new ProjectContextFileNotFound();
            }

            var projectContextFileText = File.ReadAllText(".dimmy");

            return Guid.Parse(projectContextFileText);
        }
    }

    public class ProjectContextFileNotFound : Exception
    {
    }

    public class ServiceDoesNotHaveDimmyProjectIdLabel : Exception
    {
    }
}
