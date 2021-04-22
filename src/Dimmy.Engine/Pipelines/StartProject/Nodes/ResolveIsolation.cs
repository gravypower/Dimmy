using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Dimmy.Engine.Services;
using Ductus.FluentDocker.Commands;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class ResolveIsolation:Node<IStartProjectContext>
    {
        public override int Order => -2;
        
        private readonly IHostService _host;

        public ResolveIsolation(
            IHostService host)
        {
            _host = host;
        }
        
        public override async Task DoExecute(IStartProjectContext input)
        {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            var hostVersion = System.Environment.OSVersion.Version.ToString();

            foreach (var v in input.Project.VariableDictionary)
            {
                if (!v.Key.EndsWith("Image"))
                    continue;

                var vArray = v.Key.Split(".");
                var imageConfig = _host.Host.InspectImage(v.Value);

                //list of versions
                //https://hub.docker.com/_/microsoft-windows-servercore

                var isolation = hostVersion == imageConfig.Data.Os ? "process" : "hyperv";
                input.ProjectInstance.VariableDictionary.Add($"{vArray[^2]}.Isolation", isolation);
            }
        }
    }
}