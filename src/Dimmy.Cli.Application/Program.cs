using System;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Cli.Commands;
using Dimmy.Cli.Commands.Project;
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
            Container = new Container();

            var hosts = new Hosts().Discover();
            var host = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default"); 

            if(host == null)
                Console.WriteLine("Could not find docker!");

            Container.Register(()=>host, Lifestyle.Singleton);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Container.Register(typeof(ICommandHandler<>), assemblies);
            Container.Register(typeof(IQueryHandler<,>), assemblies);
            Container.Register<IProjectService, ProjectService>();

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
