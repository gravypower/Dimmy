using System;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Cli.Commands;
using Dimmy.Cli.Commands.Project;
using Dimmy.Cli.Plugins;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Queries;
using Dimmy.Engine.Services;
using Ductus.FluentDocker.Services;
using SimpleInjector;

namespace Dimmy.Cli.Application
{
    class Program
    {
        private static readonly Container Container;

        static Program()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            Container = new Container();

            // Will be move to use plugin install with nuget 
            new VisualStudio.Plugin.Plugin().Bootstrap(Container);
            //new Sitecore.Plugin.Plugin().Bootstrap(Container);

            var loader = new Loader();
            loader.Load().RunSynchronously();

            var hosts = new Hosts().Discover();
            var host = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default"); 

            if(host == null)
                Console.WriteLine("Could not find docker!");

            Container.Register(()=> host, Lifestyle.Singleton);

            Container.Register<IProjectService, ProjectService>();
            Container.Register(typeof(ICommandHandler<>), assemblies);
            Container.Register(typeof(IQueryHandler<,>), assemblies);
            Container.Collection.Register<IProjectSubCommand>(assemblies);
            Container.Collection.Register<ICommandLineCommand>(assemblies);
            Container.Collection.Register<InitialiseSubCommand>(assemblies);

            Container.Verify();
        }


        public static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand();

            foreach (var commandLineCommand in Container.GetAllInstances<ICommandLineCommand>())
            {
                rootCommand.AddCommand(commandLineCommand.GetCommand());
            }
            
            return await rootCommand.InvokeAsync(args);
        }
    }
}
