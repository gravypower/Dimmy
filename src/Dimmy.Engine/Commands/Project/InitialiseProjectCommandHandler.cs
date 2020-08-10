using System;
using System.IO;
using Dimmy.Engine.Models;
using Dimmy.Engine.Models.Yaml;
using YamlDotNet.Serialization;

namespace Dimmy.Engine.Commands.Project
{
    public class InitialiseProjectCommandHandler : ICommandHandler<InitialiseProject>
    {
        public void Handle(InitialiseProject command)
        {
            var composeTemplate = new DockerComposeTemplate
            {
                Contents = File.ReadAllText(command.DockerComposeTemplatePath),
                FileName = Path.GetFileName(command.DockerComposeTemplatePath)
            };

            File.WriteAllText(
                Path.Combine(command.SourceCodePath, composeTemplate.FileName),
                composeTemplate.Contents);

            var dimmyProject = new ProjectYaml
            {
                Id = Guid.NewGuid(),
                ComposeTemplateFileName = composeTemplate.FileName,
                Name = command.Name,
                VariableDictionary = command.PublicVariables,
                MetaData = command.MetaData
            };

            var serializer = new Serializer();
            var dimmyProjectYaml = serializer.Serialize(dimmyProject);

            File.WriteAllText(
                Path.Combine(command.SourceCodePath, ".dimmy.yaml"),
                dimmyProjectYaml);

            var dimmyProjectInstance = new ProjectInstanceYaml
            {
                Id = dimmyProject.Id,
                Name = dimmyProject.Name,
                SourceCodeLocation = command.SourceCodePath,
                VariableDictionary = command.PrivateVariables,
            };

            var dimmyProjectInstanceYaml = serializer.Serialize(dimmyProjectInstance);

            File.WriteAllText(
                Path.Combine(command.WorkingPath, ".dimmy"),
                dimmyProjectInstanceYaml);
        }
        
    }
}