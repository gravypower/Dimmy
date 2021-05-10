using System.Runtime.InteropServices;
using Dimmy.Engine.Services.Docker;


namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class ResolveIsolation:Node<IStartProjectContext>
    {
        private readonly IDockerService _dockerService;
        public override int Order => -2;

        public ResolveIsolation(IDockerService dockerService)
        {
            _dockerService = dockerService;
        }
        
        
        public override void DoExecute(IStartProjectContext input)
        {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            var hostVersion = System.Environment.OSVersion.Version.ToString();

            foreach (var v in input.Project.VariableDictionary)
            {
                if (!v.Key.EndsWith("Image"))
                    continue;

                var vArray = v.Key.Split(".");
                var imageConfig = _dockerService.RunImageInspect(v.Value);

                //list of versions
                //https://hub.docker.com/_/microsoft-windows-servercore

                var isolation = hostVersion == imageConfig.Os ? "process" : "hyperv";
                input.ProjectInstance.VariableDictionary.Add($"{vArray[2]}.Isolation", isolation);
            }
        }
    }
}