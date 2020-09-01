using System.Runtime.InteropServices;
using Ductus.FluentDocker.Commands;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class ResolveIsolation:Node<IStartProjectContext>
    {
        public override int Order => -2;
        
        private readonly IHostService _host;

        public ResolveIsolation(IHostService host)
        {
            _host = host;
        }
        
        public override void DoExecute(IStartProjectContext input)
        {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            var hostVersion = System.Environment.OSVersion.Version.ToString();

            foreach (var service in input.DockerComposeFileConfig.ServiceDefinitions)
            {
                var imageConfig = _host.Host.InspectImage(service.Image);

                //list of versions
                //https://hub.docker.com/_/microsoft-windows-servercore
                
                if (hostVersion == imageConfig.Data.OsVersion)
                {
                    input.ProjectInstance.VariableDictionary.Add($"{service.Name}.Isolation", "process");
                }
                else
                {
                    input.ProjectInstance.VariableDictionary.Add($"{service.Name}.Isolation", "hyperv");
                }
            }
        }
    }
}