using System.Collections.Generic;
using System.Threading.Tasks;
using Dimmy.Engine.Models.Docker;

namespace Dimmy.Engine.Services.Docker
{
    public interface IDockerService
    {
        void StartContainer(string containerId);
        void StopContainer(string containerId);
        void RunPowershellInContainer(string containerId, string powershellScriptPath);
        IEnumerable<ContainerLs> RunDockerContainerLsAll();
        ImageInspect RunImageInspect(string imageId);
    }
}