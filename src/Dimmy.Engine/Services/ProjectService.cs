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

        public ProjectYamlInstanceYaml GetContextProject()
        {
            var projectContextFile = ".dimmy";
            if (!File.Exists(projectContextFile))
            {
                throw new ProjectContextFileNotFound();
            }

            var projectContext = File.ReadAllText(".dimmy");

            var deserializer = new YamlDotNet.Serialization.Deserializer();

            return deserializer.Deserialize<ProjectYamlInstanceYaml>(projectContext);
        }

        public ProjectYamlInstanceYaml NewContextProject(
            string projectName,
            string projectPath,
            string sourcePath,
            string composerTemplatePath,
            IDictionary<string, string> variableDictionary)
        {
            var instanceYaml = new ProjectYamlInstanceYaml
            {
                VariableDictionary = variableDictionary,
                Id = Guid.NewGuid(),
                Name = projectName,
                ProjectPath = projectPath,
                SourcePath = sourcePath,
                ComposeTemplate = composerTemplatePath
            };

            var serializer = new YamlDotNet.Serialization.Serializer();

            var projectContext = serializer.Serialize(instanceYaml);

            File.WriteAllText(".dimmy", projectContext);

            return instanceYaml;
        }
    }

    public class ProjectContextFileNotFound : Exception
    {
    }

    public class ServiceDoesNotHaveDimmyProjectIdLabel : Exception
    {
    }
}
