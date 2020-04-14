using System.Threading.Tasks;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Commands.Docker
{
    public class DeploymentHookCommandHandler:ICommandHandler<DeploymentHook>
    {
        private readonly IHostService _hostService;

        public DeploymentHookCommandHandler(IHostService hostService)
        {
            _hostService = hostService;
        }

        public Task Handle(DeploymentHook command)
        {
            return Task.Run(() => Run(command));
            
        }

        private static void Run(DeploymentHook command)
        {
            //container.CopyTo(
            //        @"C:\inetpub\wwwroot\bin",
            //        @"C:\projects\DIMS\src\DIMS.DeploymentHook\bin\DIMS.DeploymentHook.dll");
        }
    }
}
