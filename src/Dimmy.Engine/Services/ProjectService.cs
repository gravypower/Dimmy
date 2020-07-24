using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dimmy.Engine.Models;
using Dimmy.Engine.Models.Yaml;
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

                var containerConfiguration = container.GetConfiguration();

                projects[projectId].Services.Add(new Service
                {
                    Name = labels[DimmyDockerComposeLabels.ProjectRole],
                    ContainerId = container.Id,
                    ContainerDriver = containerConfiguration.Driver
                });
            }

            return projects.Values;
        }

        public Project GetProjectById(Guid projectId)
        {
            return RunningProjects().Single(p => p.Id == projectId);
        }

        public (ProjectInstanceYaml ProjectInstance, ProjectYaml Project) GetProject(string projectInstancePath = "")
        {
            var projectInstanceFile = Path.Combine(projectInstancePath, ".dimmy");
            if (!File.Exists(projectInstanceFile))
            {
                throw new ProjectInstanceFileFileNotFound();
            }

            var deserializer = new YamlDotNet.Serialization.Deserializer();

            var projectInstanceYaml = File.ReadAllText(projectInstanceFile);
            var projectInstance = deserializer.Deserialize<ProjectInstanceYaml>(projectInstanceYaml);

            var projectFile = Path.Combine(projectInstance.SourceCodeLocation, ".dimmy.yaml");
            if (!File.Exists(projectFile))
            {
                throw new ProjectFileFileNotFound();
            }

            var  projectYaml = File.ReadAllText(projectFile);
            var project = deserializer.Deserialize<ProjectYaml>(projectYaml);

            return (projectInstance, project);
        }
    }

    public class ProjectFileFileNotFound : Exception
    {
    }

    public class ProjectInstanceFileFileNotFound : Exception
    {
    }

    public class ServiceDoesNotHaveDimmyProjectIdLabel : Exception
    {
    }
}
