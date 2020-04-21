using System;
using System.IO;
using System.Threading.Tasks;
using Dimmy.Engine.Models.Docker;
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
            var dockerComposeFile = Path.Combine(command.ProjectFolder, "docker-compose.yml");

            if (!File.Exists(dockerComposeFile))
            {
                throw new DockerComposeFileNotFound();
            }

            var builder = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile(dockerComposeFile)
                .RemoveOrphans();

            var compositeService = builder.Build();

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            var dockerCompose = deserializer.Deserialize<DockerCompose>(File.ReadAllText(dockerComposeFile));

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
