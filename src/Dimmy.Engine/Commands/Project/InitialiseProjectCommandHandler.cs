using System;
using System.IO;
using System.Threading.Tasks;
using Dimmy.Engine.Models.Yaml;

namespace Dimmy.Engine.Commands.Project
{
    public class InitialiseProjectCommandHandler:ICommandHandler<InitialiseProject>
    {
        public Task Handle(InitialiseProject command)
        {
            return Task.Run(() => Run(command));
        }

        private static void Run(InitialiseProject command)
        {
            File.WriteAllText(
                Path.Combine(command.SourceCodePath, command.DockerComposeTemplate.FileName),
                command.DockerComposeTemplate.Contents);

            var dimmyProject = new ProjectYaml
            {
                Id = Guid.NewGuid(),
                ComposeTemplateFileName = command.DockerComposeTemplate.FileName,
                Name = command.Name,
                VariableDictionary = command.PublicVariables
            };

            var serializer = new YamlDotNet.Serialization.Serializer();
            var dimmyProjectYaml = serializer.Serialize(dimmyProject);

            File.WriteAllText(
                Path.Combine(command.SourceCodePath, ".dimmy.yaml"),
                dimmyProjectYaml);

            var dimmyProjectInstance = new ProjectInstanceYaml
            {
                Id = dimmyProject.Id,
                Name = dimmyProject.Name,
                SourceCodeLocation = command.SourceCodePath,
                VariableDictionary = command.PrivateVariables
            };

            var dimmyProjectInstanceYaml = serializer.Serialize(dimmyProjectInstance);

            File.WriteAllText(
                Path.Combine(command.WorkingPath, ".dimmy"),
                dimmyProjectInstanceYaml);
        }
    }
}
