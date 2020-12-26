using System;
using System.IO;
using Dimmy.Engine.Models.Yaml;
using YamlDotNet.Serialization;

namespace Dimmy.Engine.Pipelines.InitialiseProject.Nodes
{
    public class GenerateProjectYaml: Node<IInitialiseProjectContext>
    {
        public override int Order => 0;
        
        public override void DoExecute(IInitialiseProjectContext input)
        {
            var contents = File.ReadAllText(input.DockerComposeTemplatePath);
            var fileName = Path.GetFileName(input.DockerComposeTemplatePath);

            File.WriteAllText(Path.Combine(input.SourceCodePath, fileName), contents);

            var dimmyProject = new ProjectYaml
            {
                Id = Guid.NewGuid(),
                ComposeTemplateFileName = fileName,
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