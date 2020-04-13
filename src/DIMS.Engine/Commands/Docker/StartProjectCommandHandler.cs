using System;
using System.IO;
using Ductus.FluentDocker.Builders;

namespace DIMS.Engine.Commands.Docker
{
    public class StartProjectCommandHandler:ICommandHandler<StartProject>
    {
        public void Handle(StartProject command)
        {

            var dockerComposeFile = $"{command.ProjectFolder}\\docker-compose.yml";

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
            compositeService.Start();
        }
    }

    public class DockerComposeFileNotFound : Exception
    {
    }
}
