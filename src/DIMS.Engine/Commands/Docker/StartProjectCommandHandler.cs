using Ductus.FluentDocker.Builders;

namespace DIMS.Engine.Commands.Docker
{
    public class StartProjectCommandHandler:ICommandHandler<StartProject>
    {
        public void Handle(StartProject command)
        {
            var builder = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile($"{command.ProjectFolder}\\docker-compose.yml")
                .RemoveOrphans();

            var compositeService = builder.Build();
            compositeService.Start();
        }
    }
}
