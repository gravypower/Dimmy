using System;
using System.IO;
using System.Threading.Tasks;
using Dimmy.Engine.Models.Yaml;
using YamlDotNet.Serialization;

namespace Dimmy.Engine.Pipelines.InitialiseProject.Nodes
{
    public class GenerateProjectYaml: Node<IInitialiseProjectContext>
    {
        public override int Order => 0;
        
        public override async Task DoExecute(IInitialiseProjectContext input)
        {
            var dockerComposeFileContents = await File.ReadAllTextAsync(input.DockerComposeFilePath);
            var dockerComposeFileName = Path.GetFileName(input.DockerComposeFilePath);
            await File.WriteAllTextAsync(Path.Combine(input.SourceCodePath, dockerComposeFileName), dockerComposeFileContents);

            var environmentTemplateFileContents = await File.ReadAllTextAsync(input.EnvironmentTemplateFilePath);
            var environmentTemplateFileName = Path.GetFileName(input.EnvironmentTemplateFilePath);
            await File.WriteAllTextAsync(Path.Combine(input.SourceCodePath, environmentTemplateFileName), environmentTemplateFileContents);
            
            var dimmyProject = new ProjectYaml
            {
                Id = Guid.NewGuid(),
                DockerComposeFileName = dockerComposeFileName,
                EnvironmentTemplateFileName = environmentTemplateFileName,
                Name = input.Name,
                VariableDictionary = input.PublicVariables,
                MetaData = input.MetaData
            };

            input.ProjectYaml = dimmyProject;
            
            var dimmyProjectYaml = new Serializer().Serialize(dimmyProject);

            await File.WriteAllTextAsync(
                Path.Combine(input.SourceCodePath, ".dimmy.yaml"),
                dimmyProjectYaml);
        }
    }
}