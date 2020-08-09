using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Plugins;
using Dimmy.Engine.Queries;
using Dimmy.Engine.Queries.Plugins;
using NuGet.Protocol.Core.Types;

namespace Dimmy.Cli.Commands.Plugins
{
    public class PluginsCommandLineCommand : ICommandLineCommand
    {
        private static readonly string PluginsDirectoryPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

        private static readonly string[] DimmyProjects = {"Dimmy.Cli", "Dimmy.Engine"};

        private readonly IQueryHandler<GetRemotePlugins, IList<IPackageSearchMetadata>>
            _getRemotePluginsQueryHandler;

        private readonly ICommandHandler<InstallPlugin> _installPluginCommandHandler;

        public PluginsCommandLineCommand(
            ICommandHandler<InstallPlugin> installPluginCommandHandler,
            IQueryHandler<GetRemotePlugins, IList<IPackageSearchMetadata>> getRemotePluginsQueryHandler)
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
                new Option<bool>("--remote", "remote plugins")
            };

            pluginListCommand.Handler = CommandHandler.Create(async (bool remote) =>
            {
                if (remote)
                {
                    var plugins =  _getRemotePluginsQueryHandler.Handle(new GetRemotePlugins());

                    foreach (var plugin in plugins)
                        Console.WriteLine($"{plugin.Identity.Id} - {plugin.Identity.Version.OriginalVersion}");
                }
            });

            return pluginListCommand;
        }
        
        private Command InstallPluginCommand()
        {
            var installPluginCommand = new Command("install", "Install a dimmy plugin")
            {
                new Option<string>("--package-id", "remote plugins"),
                new Option<string>("--package-version", "remote plugins")
            };

            installPluginCommand.Handler = CommandHandler.Create(async (string packageId, string packageVersion) =>
            {
                Console.WriteLine("Plugins may be built by a 3rd party, install at own risk. Hit any key to continue.");
                Console.Read();

                if (string.IsNullOrEmpty(packageId))
                {
                    var plugins = _getRemotePluginsQueryHandler
                        .Handle(new GetRemotePlugins());


                    for (var i = 0; i < plugins.Count; i++)
                    {
                        var plugin = plugins[i];
                        Console.WriteLine($"{i} - {plugin.Identity.Id} - {plugin.Identity.Version.OriginalVersion}");
                    }

                    Console.WriteLine("Select Plugin to install");
                    var selectPlugin = int.Parse(Console.ReadLine());
                    
                    var selectedPlugin = plugins[selectPlugin];
                    packageId = selectedPlugin.Identity.Id;
                    packageVersion = selectedPlugin.Identity.Version.OriginalVersion;
                }
                
            
                _installPluginCommandHandler.Handle(new InstallPlugin
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