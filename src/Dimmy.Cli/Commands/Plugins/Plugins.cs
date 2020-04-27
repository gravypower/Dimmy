using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Plugins;
using Dimmy.Engine.Queries;
using Dimmy.Engine.Queries.Plugins;
using NuGet.Protocol.Core.Types;

namespace Dimmy.Cli.Commands.Plugins
{
    public class Plugins : ICommandLineCommand
    {
        private static readonly string PluginsDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

        private static readonly string[] DimmyProjects = { "Dimmy.Cli", "Dimmy.Engine" };

        private readonly ICommandHandler<InstallPlugin> _installPluginCommandHandler;
        private readonly IQueryHandler<GetRemotePlugins, IAsyncEnumerable<IPackageSearchMetadata>> _getRemotePluginsQueryHandler;

        public Plugins(
            ICommandHandler<InstallPlugin> installPluginCommandHandler,
            IQueryHandler<GetRemotePlugins, IAsyncEnumerable<IPackageSearchMetadata>> getRemotePluginsQueryHandler)
        {
            _installPluginCommandHandler = installPluginCommandHandler;
            _getRemotePluginsQueryHandler = getRemotePluginsQueryHandler;
        }

        public Command GetCommand()
        {
            var projectsCommand = new Command("plugins");

            projectsCommand.AddCommand(PluginListCommand());
            projectsCommand.AddCommand(InstallPluginCommand());
            projectsCommand.AddAlias("plugins");

            return projectsCommand;
        }

        private Command PluginListCommand()
        {
            var pluginListCommand = new Command("ls", "Lists dimmy plugins")
            {
                new Option<bool>("--remote", "remote plugins"),
            };

            pluginListCommand.Handler = CommandHandler.Create(async (bool remote) =>
            {
                if (remote)
                {
                    var plugins = await _getRemotePluginsQueryHandler.Handle(new GetRemotePlugins());

                    await foreach (var plugin in plugins)
                    {
                        Console.WriteLine($"{plugin.Identity.Id} - {plugin.Identity.Version.OriginalVersion}");
                    }
                }
            });

            return pluginListCommand;
        }


        private Command InstallPluginCommand()
        {
            var installPluginCommand = new Command("install", "Install a dimmy plugin")
            {
                new Option<string>("--package-id", "remote plugins"),
                new Option<string>("--package-version", "remote plugins"),
            };

            installPluginCommand.Handler = CommandHandler.Create(async (string packageId, string packageVersion) =>
            {
                await _installPluginCommandHandler.Handle(new InstallPlugin
                    {
                        PackageId = packageId,
                        PackageVersion = packageVersion,
                        PackageFramework = "netstandard2.1",
                        InstallDirectory = PluginsDirectoryPath,
                        OmitDependencies = DimmyProjects
                    });
         
            });

            return installPluginCommand;
        }

    }
}
