using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dimmy.Engine.Models.Yaml.DockerCompose;
using Dimmy.Engine.Services;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Compose;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Dimmy.Engine.Commands.Docker
{
    public class StartProjectCommandHandler : ICommandHandler<StartProject>
    {
        private readonly IProjectService _projectService;

        public StartProjectCommandHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public Task Handle(StartProject command)
        {
            return Task.Run(() => Run(command));
        }

        private void Run(StartProject command)
        {
            if (!File.Exists(command.DockerComposeFilePath)) throw new DockerComposeFileNotFound();

            var builder = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile(command.DockerComposeFilePath)
                .RemoveOrphans();

            var compositeService = builder.Build();

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            
            var dockerCompose = 
                deserializer.Deserialize<DockerComposeYaml>(File.ReadAllText(command.DockerComposeFilePath));


            foreach (var dockerComposeService in dockerCompose.Services)
            {
                if(dockerComposeService.Value.Volumes == null)
                    continue;
                
                foreach (var volume in dockerComposeService.Value.Volumes)
                {
                    var hostFolderName = string.Empty;
                    if (volume is string)
                    {
                        var volumeParts = volume.Split(':');
                        hostFolderName = volumeParts[0];
                    }
                    else if (volume is IDictionary<object, object>)
                    {
                        if (volume["type"] == "bind")
                        {
                            hostFolderName = volume["source"];
                        }
                    }

                    if(string.IsNullOrEmpty(hostFolderName))
                        continue;

                    var composeFilePath = Path.GetDirectoryName(command.DockerComposeFilePath);

                    var hostFolderPath = Path.Combine(composeFilePath, hostFolderName);
                    var exists = Directory.Exists(hostFolderPath);

                    if (!exists)
                        Directory.CreateDirectory(hostFolderPath);
                }
            }



            //compositeService.Start();
        }
    }

    public class DockerComposeFileNotFound : Exception
    {
    }
}