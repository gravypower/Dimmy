using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Octostache;

namespace Dimmy.Engine.Pipelines.GenerateEnvironmentFile.Nodes
{
    public class GenerateEnvironmentFile : Node<IGenerateEnvironmentFileContext>
    {
        public override async Task DoExecute(IGenerateEnvironmentFileContext input)
        {
            var variableDictionary = new VariableDictionary();
            variableDictionary.Set("Project.Name", input.ProjectInstance.Name);
            variableDictionary.Set("Project.Id", $"{input.ProjectInstance.Id:N}");
            variableDictionary.Set("Project.WorkingPath", Regex.Escape(input.WorkingPath));

            foreach (var (key, value) in input.ProjectInstance.VariableDictionary) variableDictionary.Set(key, value);

            foreach (var (key, value) in input.Project.VariableDictionary) variableDictionary.Set(key, value);

            var environmentTemplateFilePath = Path.Combine(
                input.ProjectInstance.SourceCodeLocation,
                input.Project.EnvironmentTemplateFileName);
            
            var environmentTemplate = await File.ReadAllTextAsync(environmentTemplateFilePath);

            var environmentFile = variableDictionary.Evaluate(environmentTemplate, out var error);

            if (!string.IsNullOrEmpty(error))
                throw new DockerComposeGenerationFailed(error);

            var environmentFileNamePath = Path.Combine(input.WorkingPath, ".env");

            await File.WriteAllTextAsync(environmentFileNamePath, environmentFile);
        }
    }
}