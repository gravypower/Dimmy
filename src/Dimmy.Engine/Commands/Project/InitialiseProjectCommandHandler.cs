using System;
using System.IO;
using System.Threading.Tasks;
using Dimmy.Engine.Models;

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
                $"{command.SourceCodePath}\\{command.DockerComposeTemplate.FileName}",
                command.DockerComposeTemplate.Contents);

            var dimmyProject = new ProjectYaml
            {
                Id = Guid.NewGuid(),
                ComposeTemplate = command.DockerComposeTemplate.FileName,
                Name = command.Name,
                VariableDictionary = command.PublicVariables
            };

            var serializer = new YamlDotNet.Serialization.Serializer();
            var dimmyProjectYaml = serializer.Serialize(dimmyProject);

            File.WriteAllText(
                $"{command.SourceCodePath}\\.dimmy.yaml",
                dimmyProjectYaml);

            var dimmyProjectInstance = new ProjectInstanceYaml
            {
                ComposeTemplate = command.DockerComposeTemplate.Contents,
                Id = dimmyProject.Id,
                Name = dimmyProject.Name,
                WorkingPath = command.WorkingPath,
                SourceCodeLocation = command.SourceCodePath,
                VariableDictionary = command.PrivateVariables
            };

            var dimmyProjectInstanceYaml = serializer.Serialize(dimmyProjectInstance);

            File.WriteAllText(
                $"{command.WorkingPath}\\.dimmy",
                dimmyProjectInstanceYaml);
        }
    }
}
