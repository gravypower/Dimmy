using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dimmy.Engine.Models;
using Dimmy.Engine.Models.Yaml;
using Ductus.FluentDocker.Services;
using YamlDotNet.Serialization;

namespace Dimmy.Engine.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IHostService _hostService;

        public ProjectService(IHostService hostService)
        {
            _hostService = hostService;
        }

        public IList<Project> RunningProjects()
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

                var canParseProjectId = Guid.TryParse(labels[DimmyDockerComposeLabels.ProjectId], out var projectId);

                if (!canParseProjectId)
                    throw new ContainerDoesNotHaveValidDimmyProjectId();
                
                var projectName = labels[DimmyDockerComposeLabels.ProjectName];
                var projectWorkingPath = labels[DimmyDockerComposeLabels.ProjectWorkingPath];

                if (!projects.ContainsKey(projectId))
                    projects.Add(projectId, new Project
                    {
                        Name = projectName,
                        Id = projectId,
                        WorkingPath = projectWorkingPath
                    });

                projects[projectId].Services.Add(new Service
                {
                    Name = labels[DimmyDockerComposeLabels.ProjectRole],
                    ContainerId = container.Id
                });
            }

            return projects.Values.ToList();
        }

        public Project GetProjectById(Guid projectId)
        {
            return RunningProjects().Single(p => p.Id == projectId);
        }

        public (ProjectInstanceYaml ProjectInstance, ProjectYaml Project) GetProject(string projectInstancePath = "")
        {
            var projectInstanceFile = Path.Combine(projectInstancePath, ".dimmy");
            if (!File.Exists(projectInstanceFile)) throw new ProjectInstanceFileFileNotFound();

            var deserializer = new Deserializer();

            var projectInstanceYaml = File.ReadAllText(projectInstanceFile);
            var projectInstance = deserializer.Deserialize<ProjectInstanceYaml>(projectInstanceYaml);

            var projectFile = Path.Combine(projectInstance.SourceCodeLocation, ".dimmy.yaml");
            if (!File.Exists(projectFile)) throw new ProjectFileFileNotFound();

            var projectYaml = File.ReadAllText(projectFile);
            var project = deserializer.Deserialize<ProjectYaml>(projectYaml);

            return (projectInstance, project);
        }
    }

    public class ContainerDoesNotHaveValidDimmyProjectId : Exception
    {
    }
}