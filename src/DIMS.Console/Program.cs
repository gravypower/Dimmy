using System;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using DIMS.CLI.RootCommands;
using DIMS.CLI.RootCommands.Project;
using DIMS.Engine.Commands;
using DIMS.Engine.Queries;
using DIMS.Engine.Services;
using Ductus.FluentDocker.Services;
using SimpleInjector;

namespace DIMS.CLI
{
    class Program
    {
        private static readonly Container Container;
        static Program()
        {
            Container = new Container();

            var hosts = new Hosts().Discover();
            var host = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");
            Container.Register(()=>host, Lifestyle.Singleton);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Container.Register(typeof(ICommandHandler<>), assemblies);
            Container.Register(typeof(IQueryHandler<,>), assemblies);
            Container.Register<IProjectService, ProjectService>();

            Container.Collection.Register<IProjectSubCommand>(assemblies);
            Container.Collection.Register<ICommandLineCommand>(assemblies);

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
