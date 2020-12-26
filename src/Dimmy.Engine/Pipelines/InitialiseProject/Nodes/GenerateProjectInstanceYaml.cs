using System.IO;
using Dimmy.Engine.Models.Yaml;
using YamlDotNet.Serialization;

namespace Dimmy.Engine.Pipelines.InitialiseProject.Nodes
{
    public class GenerateProjectInstanceYaml: Node<IInitialiseProjectContext>
    {
        public override int Order => 1;

        public override void DoExecute(IInitialiseProjectContext input)
        {
            var dimmyProjectInstance = new ProjectInstanceYaml
            {
                Id = input.ProjectYaml.Id,
                Name = input.ProjectYaml.Name,
                SourceCodeLocation = input.SourceCodePath,
                VariableDictionary = input.PrivateVariables,
            };

            var dimmyProjectInstanceYaml =  new Serializer().Serialize(dimmyProjectInstance);

            File.WriteAllText(
                Path.Combine(input.WorkingPath, ".dimmy"),
                dimmyProjectInstanceYaml);
        }
    }
}