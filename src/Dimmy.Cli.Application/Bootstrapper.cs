using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dimmy.Cli.Commands.Plugins;
using Dimmy.Cli.Commands.Project;
using Dimmy.Cli.Commands.Project.SubCommands;
using Dimmy.Cli.Commands.Projects;
using Dimmy.Engine.Commands;
using Dimmy.Engine.NuGet;
using Dimmy.Engine.Queries;
using Dimmy.Engine.Services;
using Dimmy.Engine.Services.Projects;
using Ductus.FluentDocker.Services;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using ICommand = Dimmy.Cli.Commands.ICommand;

namespace Dimmy.Cli.Application
{
    public class Bootstrapper
    {
        private static readonly Container Container = new Container();

        public static Container Bootstrap()
        {
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            var assemblies = ResolveAssemblies();
            RegisterDockerHost();

            //Nuget
            Container.Register<ILogger>(() => new TextWriterLogger(Console.Out));
            Container.Register(() => new SourceCacheContext(), Lifestyle.Scoped);
            Container.Register(() => Settings.LoadDefaultSettings(null), Lifestyle.Scoped);
            Container.Register<INugetService, NugetService>();

            Container.Register<IProjectService, ProjectService>();
            Container.Register<ICertificateService, CertificateService>();
            
            Container.Collection.Register<IProjectSubCommand>(assemblies);
            
            
            Container.Collection.Register(typeof(ICommand), new []
            {
                typeof(Project),
                typeof(Projects),
                typeof(Plugins)
            });

            Container.Collection.Register<InitialiseSubCommand>(assemblies);

            Container.Register(typeof(ICommandHandler<>), assemblies);
            Container.Register(typeof(IQueryHandler<,>), assemblies);

            Container.Verify();

            return Container;
        }

        private static List<Assembly> ResolveAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            var pluginAssemblies = PluginLoader.Load(Container);
            assemblies.AddRange(pluginAssemblies);
            return assemblies;
        }

        private static void RegisterDockerHost()
        {
            var hosts = new Hosts().Discover();
            var host = hosts
                           .FirstOrDefault(x => x.IsNative)
                       ?? hosts.FirstOrDefault(x => x.Name == "default");

            if (host == null)
                Console.WriteLine("Could not find docker!");

            Container.Register(() => host, Lifestyle.Singleton);
        }
    }
}