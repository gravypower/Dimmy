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
        
        public override void DoExecute(IInitialiseProjectContext input)
        {
            var dockerComposeFileContents = File.ReadAllText(input.DockerComposeFilePath);
            var dockerComposeFileName = Path.GetFileName(input.DockerComposeFilePath);
            File.WriteAllText(Path.Combine(input.SourceCodePath, dockerComposeFileName), dockerComposeFileContents);

            var environmentTemplateFileContents = File.ReadAllText(input.EnvironmentTemplateFilePath);
            var environmentTemplateFileName = Path.GetFileName(input.EnvironmentTemplateFilePath);
            File.WriteAllText(Path.Combine(input.SourceCodePath, environmentTemplateFileName), environmentTemplateFileContents);
            
            var dimmyProject = new ProjectYaml
            {
                Id = Guid.NewGuid(),
                DockerComposeTemplateFileName = dockerComposeFileName,
                EnvironmentTemplateFileName = environmentTemplateFileName,
                Name = input.Name,
                VariableDictionary = input.PublicVariables,
                MetaData = input.MetaData
            };

            input.ProjectYaml = dimmyProject;
            
            var dimmyProjectYaml = new Serializer().Serialize(dimmyProject);

            File.WriteAllText(
                Path.Combine(input.SourceCodePath, ".dimmy.yaml"),
                dimmyProjectYaml);
        }
    }
}