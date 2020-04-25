using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Cli.Commands;
using Dimmy.Cli.Commands.Project;
using Dimmy.Cli.Plugins;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Queries;
using Dimmy.Engine.Services;
using Ductus.FluentDocker.Services;
using McMaster.NETCore.Plugins;
using SimpleInjector;

namespace Dimmy.Cli.Application
{
    class Program
    {
        private static readonly Container Container = new Container();

        public static async Task<int> Main(string[] args)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Will be move to use plugin install with nuget 
            new VisualStudio.Plugin.Plugin().Bootstrap(Container);
            //new Sitecore.Plugin.Plugin().Bootstrap(Container);

            var loader = new Loader();
            var list = await loader.Load();

            // create plugin loaders
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
            foreach (var dir in Directory.GetDirectories(pluginsDir))
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (!File.Exists(pluginDll)) continue;

                var pluginLoader = PluginLoader.CreateFromAssemblyFile(
                    pluginDll,
                    new[] {typeof(IPlugin)});


                var pluginType = pluginLoader.LoadDefaultAssembly().GetTypes()
                    .Single(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract);

                var plugin = (IPlugin) Activator.CreateInstance(pluginType);

                plugin.Bootstrap(Container);
            }


            var hosts = new Hosts().Discover();
            var host = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");



            if (host == null)
                Console.WriteLine("Could not find docker!");

            Container.Register(() => host, Lifestyle.Singleton);

            Container.Register<IProjectService, ProjectService>();
            Container.Register(typeof(ICommandHandler<>), assemblies);
            Container.Register(typeof(IQueryHandler<,>), assemblies);
            Container.Collection.Register<IProjectSubCommand>(assemblies);
            Container.Collection.Register<ICommandLineCommand>(assemblies);
            Container.Collection.Register<InitialiseSubCommand>(assemblies);

            Container.Verify();

            var rootCommand = new RootCommand();

            foreach (var commandLineCommand in Container.GetAllInstances<ICommandLineCommand>())
            {
                rootCommand.AddCommand(commandLineCommand.GetCommand());
            }
            
            return await rootCommand.InvokeAsync(args);
        }
    }
}
