using System;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using DIMS.CLI.RootCommands;
using DIMS.Engine.Commands;
using DIMS.Engine.Queries;
using DIMS.Engine.Services;
using Ductus.FluentDocker.Services;
using SimpleInjector;

namespace DIMS.CLI
{
    class Program
    {
        private static readonly Container container;
        static Program()
        {
            container = new Container();

            var hosts = new Hosts().Discover();
            var host = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");
            container.Register(()=>host, Lifestyle.Singleton);

            container.Register(
                typeof(ICommandHandler<>),
                AppDomain.CurrentDomain.GetAssemblies());

            container.Register(
                typeof(IQueryHandler<,>),
                AppDomain.CurrentDomain.GetAssemblies());

            container.Register<IProjectService, ProjectService>();

            container.Verify();
        }

        public static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                container.GetInstance<Projects>(),
                container.GetInstance<Project>()
            };

            return await rootCommand.InvokeAsync(args);


           
            //var generateComposeYaml = new GenerateComposeYaml
            //{
            //    Topology = new XpTopology(),
            //    ProjectFolder = @"C:\clients\SomeClient",
            //    LicenseStream = File.OpenRead(@"C:\license.xml"),
            //    ProjectName = "SomeClient"
            //};

            //var generateComposeYamlCommandHandler = new GenerateComposeYamlCommandHandler();

            //generateComposeYamlCommandHandler.Handle(generateComposeYaml);

            //var startSitecoreInstance = new StartProject
            //{
            //    ProjectFolder = @"C:\clients\SomeClient"
            //};

            //var startSitecoreInstanceCommandHandler = new StartProjectCommandHandler();
            //startSitecoreInstanceCommandHandler.Handle(startSitecoreInstance);

           

        }
    }
}
