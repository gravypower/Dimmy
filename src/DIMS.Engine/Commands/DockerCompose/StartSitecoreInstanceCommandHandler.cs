using Ductus.FluentDocker.Builders;

namespace DIMS.Engine.Commands.DockerCompose
{
    public class StartSitecoreInstanceCommandHandler:ICommandHandler<StartSitecoreInstance>
    {
        public void Handle(StartSitecoreInstance command)
        {
            var xp = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile($"{command.ProjectFolder}\\docker-compose.yml")
                .RemoveOrphans();

            var xpCompositeService = xp.Build();
            xpCompositeService.Start();
        }
    }
}
