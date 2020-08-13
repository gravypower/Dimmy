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
    public class Plugins : Command<PluginsArgument>
    {
        private static readonly string PluginsDirectoryPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");

        private static readonly string[] DimmyProjects = {"Dimmy.Cli", "Dimmy.Engine"};

        private readonly IQueryHandler<GetRemotePlugins, IList<IPackageSearchMetadata>>
            _getRemotePluginsQueryHandler;

        private readonly ICommandHandler<InstallPlugin> _installPluginCommandHandler;

        public Plugins(
            ICommandHandler<InstallPlugin> installPluginCommandHandler,
            IQueryHandler<GetRemotePlugins, IList<IPackageSearchMetadata>> getRemotePluginsQueryHandler)
        {
            _installPluginCommandHandler = installPluginCommandHandler;
            _getRemotePluginsQueryHandler = getRemotePluginsQueryHandler;
        }

        public override Command BuildCommand()
        {
            var projectsCommand = new Command("plugins");

            projectsCommand.AddCommand(PluginListCommand());
            projectsCommand.AddCommand(InstallPluginCommand());
            projectsCommand.AddAlias("plugins");

            return projectsCommand;
        }

        public override void CommandAction(PluginsArgument arg)
        {
        }

        private Command PluginListCommand()
        {
            var pluginListCommand = new Command("ls", "Lists dimmy plugins")
            {
                new Option<bool>("--remote", "remote plugins")
            };

            pluginListCommand.Handler = CommandHandler.Create((bool remote) =>
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

            installPluginCommand.Handler = CommandHandler.Create((string packageId, string packageVersion) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Plugins may be built by a 3rd party, install at own risk.");
                Console.ResetColor();

                if (string.IsNullOrEmpty(packageId))
                {
                    var plugins = _getRemotePluginsQueryHandler
                        .Handle(new GetRemotePlugins());
                    
                    for (var i = 0; i < plugins.Count; i++)
                    {
                        var plugin = plugins[i];
                        Console.WriteLine($"{i + 1} - {plugin.Identity.Id} - {plugin.Identity.Version.OriginalVersion}");
                    }
                    
                    Console.Write("Select Plugin to install:");
                    var selectPlugin = Console.ReadLine();
                    
                    var selectedPlugin = plugins[int.Parse(selectPlugin) - 1];
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