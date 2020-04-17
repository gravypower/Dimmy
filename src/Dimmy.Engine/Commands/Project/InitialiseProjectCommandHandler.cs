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
                $"{command.SourceCodeLocation}\\{command.DockerComposeTemplate.FileName}",
                command.DockerComposeTemplate.Contents);

            var dimmyProject = new ProjectYaml
            {
                Id = Guid.NewGuid(),
                ComposeTemplate = command.DockerComposeTemplate.FileName
            };

            var serializer = new YamlDotNet.Serialization.Serializer();
            var dimmyProjectYaml = serializer.Serialize(dimmyProject);

            File.WriteAllText(
                $"{command.SourceCodeLocation}\\.dimmy.yaml",
                dimmyProjectYaml);

            var dimmyProjectInstance = new ProjectYamlInstanceYaml
            {
                ComposeTemplate = command.DockerComposeTemplate.Contents,
                Id = dimmyProject.Id,
                Name = dimmyProject.Name,
                ProjectPath = command.ProjectLocation,
                SourceCodeLocation = command.SourceCodeLocation
            };

            var dimmyProjectInstanceYaml = serializer.Serialize(dimmyProjectInstance);

            File.WriteAllText(
                $"{command.ProjectLocation}\\.dimmy",
                dimmyProjectInstanceYaml);
        }
    }
}
