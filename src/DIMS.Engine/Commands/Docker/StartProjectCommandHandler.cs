using System;
using System.IO;
using System.Threading.Tasks;
using Ductus.FluentDocker.Builders;

namespace DIMS.Engine.Commands.Docker
{
    public class StartProjectCommandHandler:ICommandHandler<StartProject>
    {
        public Task Handle(StartProject command)
        {
            return Task.Run(()=> Run(command));
        }

        private static void Run(StartProject command)
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
