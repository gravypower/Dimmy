using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Dimmy.Engine.Pipelines.PauseProject;
using Dimmy.Engine.Services.Projects;
using Ductus.FluentDocker.Services;
using Octostache;

namespace Dimmy.Engine.Pipelines.GenerateComposeYaml.Nodes
{
    public class GenerateComposeYaml : Node<IGenerateComposeYamlContext>
    {
        private readonly IHostService _hostService;
        private readonly IProjectService _projectService;

        public GenerateComposeYaml(
            IHostService hostService,
            IProjectService projectService)
        {
            _hostService = hostService;
            _projectService = projectService;
        }

        public override void DoExecute(IGenerateComposeYamlContext input)
        {
            var variableDictionary = new VariableDictionary();
            variableDictionary.Set("Project.Name", input.ProjectInstance.Name);
            variableDictionary.Set("Project.Id", $"{input.ProjectInstance.Id:N}");
            variableDictionary.Set("Project.WorkingPath", Regex.Escape(input.WorkingPath));

            foreach (var (key, value) in input.ProjectInstance.VariableDictionary) variableDictionary.Set(key, value);

            foreach (var (key, value) in input.Project.VariableDictionary) variableDictionary.Set(key, value);

            var templateFilePath = Path.Combine(input.ProjectInstance.SourceCodeLocation,
                input.Project.ComposeTemplateFileName);
            var template = File.ReadAllText(templateFilePath);

            var dockerCompose = variableDictionary.Evaluate(template, out var error);

            if (!string.IsNullOrEmpty(error))
                throw new DockerComposeGenerationFailed(error);

            var dockerComposeFile = Path.Combine(input.WorkingPath, "docker-compose.yml");

            File.WriteAllText(dockerComposeFile, dockerCompose);
        }
    }
}