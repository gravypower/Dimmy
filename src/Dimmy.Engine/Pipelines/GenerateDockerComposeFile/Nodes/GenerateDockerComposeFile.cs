using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Octostache;

namespace Dimmy.Engine.Pipelines.GenerateDockerComposeFile.Nodes
{
    public class GenerateDockerComposeFile : Node<IGenerateDockerComposeFileContext>
    {
        public override void DoExecute(IGenerateDockerComposeFileContext input)
        {
            var variableDictionary = new VariableDictionary();
            variableDictionary.Set("Project.Name", input.ProjectInstance.Name);
            variableDictionary.Set("Project.Id", $"{input.ProjectInstance.Id:N}");
            variableDictionary.Set("Project.WorkingPath", Regex.Escape(input.WorkingPath));

            foreach (var (key, value) in input.ProjectInstance.VariableDictionary) variableDictionary.Set(key, value);

            foreach (var (key, value) in input.Project.VariableDictionary) variableDictionary.Set(key, value);

            var templateFilePath = Path.Combine(
                input.ProjectInstance.SourceCodeLocation,
                input.Project.DockerComposeTemplateFileName);
            
            var template = File.ReadAllText(templateFilePath);

            var dockerCompose = variableDictionary.Evaluate(template, out var error);

            if (!string.IsNullOrEmpty(error))
                throw new FileGenerationFailed(error);

            var environmentFileNamePath = Path.Combine(input.WorkingPath, "docker-compose.yml");

            File.WriteAllText(environmentFileNamePath, dockerCompose);
        }
    }
}