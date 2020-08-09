using System.Threading.Tasks;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Commands.Docker
{
    public class DeploymentHookCommandHandler : ICommandHandler<DeploymentHook>
    {
        private readonly IHostService _hostService;

        public DeploymentHookCommandHandler(IHostService hostService)
        {
            _hostService = hostService;
        }

        public void Handle(DeploymentHook command)
        {
            //container.CopyTo(
            //        @"C:\inetpub\wwwroot\bin",
            //        @"C:\projects\DIMS\src\DIMS.DeploymentHook\bin\DIMS.DeploymentHook.dll");
        }
    }
}