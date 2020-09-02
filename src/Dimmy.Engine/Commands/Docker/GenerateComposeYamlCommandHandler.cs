using System.IO;
using System.Text.RegularExpressions;
using Octostache;

namespace Dimmy.Engine.Commands.Docker
{
    public class GenerateComposeYamlCommandHandler : ICommandHandler<GenerateComposeYaml>
    {
        public void Handle(GenerateComposeYaml command)
        {
            var variableDictionary = new VariableDictionary();
            variableDictionary.Set("Project.Name", command.ProjectInstance.Name);
            variableDictionary.Set("Project.Id", $"{command.ProjectInstance.Id:N}");
            variableDictionary.Set("Project.WorkingPath", Regex.Escape(command.WorkingPath));

            foreach (var (key, value) in command.ProjectInstance.VariableDictionary) variableDictionary.Set(key, value);

            foreach (var (key, value) in command.Project.VariableDictionary) variableDictionary.Set(key, value);
            
            var templateFilePath = Path.Combine(command.ProjectInstance.SourceCodeLocation, command.Project.ComposeTemplateFileName);
            var template = File.ReadAllText(templateFilePath);

            var dockerCompose = variableDictionary.Evaluate(template, out var error);

            if (!string.IsNullOrEmpty(error))
                throw new DockerComposeGenerationFailed(error);

            var dockerComposeFile = Path.Combine(command.WorkingPath, "docker-compose.yml");

            File.WriteAllText(dockerComposeFile, dockerCompose);
        }
    }
}