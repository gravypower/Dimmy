﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dimmy.Engine.Services.Docker
{
    public interface IDockerService
    {
        Task StartContainer(string containerId);
        Task StopContainer(string containerId);
        Task RunPowershellInContainer(string containerId, string powershellScriptPath);

        Task<IList<ContainerLs>> RunDockerContainerLsAll();
    }
}