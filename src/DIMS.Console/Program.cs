using System.IO;
using System.Linq;
using DIMS.Engine;
using DIMS.Engine.Commands.DockerCompose;
using DIMS.Engine.Queries.Projects;
using Ductus.FluentDocker.Services;

namespace DIMS.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var generateComposeYaml = new GenerateComposeYaml
            {
                Topology = new XpTopology(),
                ProjectFolder = @"C:\clients\SomeClient",
                LicenseStream = File.OpenRead(@"C:\license.xml")
            };

            var generateComposeYamlCommandHandler = new GenerateComposeYamlCommandHandler();

            generateComposeYamlCommandHandler.Handle(generateComposeYaml);

            var startSitecoreInstance = new StartSitecoreInstance
            {
                ProjectFolder = @"C:\clients\SomeClient"
            };

            var startSitecoreInstanceCommandHandler = new StartSitecoreInstanceCommandHandler();
            startSitecoreInstanceCommandHandler.Handle(startSitecoreInstance);


            var getRunningProjects = new GetRunningProjects();

            var hosts = new Hosts().Discover();
            var host = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");
            var getRunningProjectsQueryHandler = new GetRunningProjectsQueryHandler(host);

            getRunningProjectsQueryHandler.Handle(getRunningProjects);

        }
    }
}
