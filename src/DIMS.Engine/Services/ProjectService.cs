using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DIMS.Engine.Models;
using Ductus.FluentDocker.Services;

namespace DIMS.Engine.Services
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

                if (!labels.ContainsKey(DimsDockerComposeLabels.Project))
                    continue;

                if (!labels.ContainsKey(DimsDockerComposeLabels.ProjectId))
                    throw new ServiceDoesNotHaveDIMSProjectIdLabel();

                var projectId = Guid.Parse(labels[DimsDockerComposeLabels.ProjectId]);

                var projectName = labels[DimsDockerComposeLabels.ProjectName];

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
                    Name = labels[DimsDockerComposeLabels.ProjectRole],
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
            var configFile = File.ReadAllText(".dims");

            return Guid.Parse(configFile);
        }
    }

    public class ServiceDoesNotHaveDIMSProjectIdLabel : Exception
    {
    }
}
