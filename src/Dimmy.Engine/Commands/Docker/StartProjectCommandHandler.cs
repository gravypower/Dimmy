using System;
using System.IO;
using System.Threading.Tasks;
using Dimmy.Engine.Models.Yaml.DockerCompose;
using Dimmy.Engine.Services;
using Ductus.FluentDocker.Builders;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Dimmy.Engine.Commands.Docker
{
    public class StartProjectCommandHandler:ICommandHandler<StartProject>
    {
        private readonly IProjectService _projectService;

        public StartProjectCommandHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public Task Handle(StartProject command)
        {
            return Task.Run(()=> Run(command));
        }

        private void Run(StartProject command)
        {
            if (!File.Exists(command.DockerComposeFilePath))
            {
                throw new DockerComposeFileNotFound();
            }

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

            var dockerCompose = deserializer.Deserialize<DockerComposeYaml>(File.ReadAllText(command.DockerComposeFilePath));

            foreach (var dockerComposeService in dockerCompose.Services)
            {
                foreach (var volume in dockerComposeService.Value.Volumes)
                {
                    var volumeParts = volume.Split(':');

                    var hostPath = $"{volumeParts[0]}:{ volumeParts[1]}";
                    var exists = Directory.Exists(hostPath);

                    if (!exists)
                        Directory.CreateDirectory(hostPath);
                }
            }



            compositeService.Start();
        }
    }

    public class DockerComposeFileNotFound : Exception
    {
    }
}
