using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dimmy.Cli.Commands;
using Dimmy.Cli.Commands.Logo;
using Dimmy.Cli.Commands.Plugins;
using Dimmy.Cli.Commands.Project;
using Dimmy.Cli.Commands.Project.SubCommands;
using Dimmy.Cli.Commands.Projects;
using Dimmy.Cli.Commands.Utility;
using Dimmy.Engine.NuGet;
using Dimmy.Engine.Pipelines;
using Dimmy.Engine.Queries;
using Dimmy.Engine.Services;
using Dimmy.Engine.Services.Docker;
using Dimmy.Engine.Services.Hosts;
using Dimmy.Engine.Services.Nuget;
using Dimmy.Engine.Services.Projects;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace Dimmy.Cli.Application
{
    public class Bootstrapper
    {
        private static readonly Container Container = new Container();

        public static Container Bootstrap()
        {
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            var assemblies = ResolveAssemblies();

            //Nuget
            Container.Register<ILogger>(() => new TextWriterLogger(Console.Out));
            Container.Register(() => new SourceCacheContext(), Lifestyle.Scoped);
            Container.Register(() => Settings.LoadDefaultSettings(null), Lifestyle.Scoped);
            Container.Register<INugetService, NugetService>();

            Container.Register<IProjectService, ProjectService>();
            Container.Register<IDockerComposeParser, DockerComposeParser>();
            Container.Register<ICertificateService, CertificateService>();
            Container.Register<IHostsFileService, HostsFileService>();
            Container.Register<IDockerService, DockerService>();
            
            
            Container.Collection.Register<IProjectSubCommand>(assemblies);
            
            Container.Register(typeof(Pipeline<,>), assemblies);
            Container.Collection.Register(typeof(ICommand), new []
            {
                typeof(Project),
                typeof(Projects),
                typeof(Plugins),
                typeof(Logo),
                typeof(Utility),
            });

            Container.Collection.Register<InitialiseSubCommand>(assemblies);
            
            Container.Register(typeof(IQueryHandler<,>), assemblies);
            
            Container.Collection.Register(typeof(Node<>), assemblies);

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
    }
}