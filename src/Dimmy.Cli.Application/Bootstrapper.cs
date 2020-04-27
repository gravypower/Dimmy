using System;
using System.Linq;
using Dimmy.Cli.Commands;
using Dimmy.Cli.Commands.Project;
using Dimmy.Engine.Commands;
using Dimmy.Engine.NuGet;
using Dimmy.Engine.Queries;
using Dimmy.Engine.Services;
using Ductus.FluentDocker.Services;
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

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            var pluginAssemblies = PluginLoader.Load(Container);

            assemblies.AddRange(pluginAssemblies);

            var hosts = new Hosts().Discover();
            var host = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");

            if (host == null)
                Console.WriteLine("Could not find docker!");

            Container.Register(() => host, Lifestyle.Singleton);

            Container.Register<ILogger>(() => new TextWriterLogger(Console.Out));
            Container.Register(() => new SourceCacheContext(), Lifestyle.Scoped);

            Container.Register(() => Settings.LoadDefaultSettings(root: null), Lifestyle.Scoped);
            Container.Register<INugetService, NugetService>();

            

            Container.Register<IProjectService, ProjectService>();
            Container.Register(typeof(ICommandHandler<>), assemblies);
            Container.Register(typeof(IQueryHandler<,>), assemblies);
            Container.Collection.Register<IProjectSubCommand>(assemblies);
            Container.Collection.Register<ICommandLineCommand>(assemblies);
            Container.Collection.Register<InitialiseSubCommand>(assemblies);

            Container.Verify();

            return Container;
        }
    }
}
