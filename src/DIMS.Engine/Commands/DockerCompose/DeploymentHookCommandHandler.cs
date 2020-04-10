﻿using System;
using System.Linq;
using Ductus.FluentDocker.Services;

namespace DIMS.Engine.Commands.DockerCompose
{
    public class DeploymentHookCommandHandler:ICommandHandler<DeploymentHook>
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
