using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Cli.Commands.Project;
using Dimmy.Cli.Extensions;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Project;
using Dimmy.Engine.Models;
using Dimmy.Engine.Services;
using Dimmy.Sitecore.Plugin.Topologies;

namespace Dimmy.Sitecore.Plugin
{
    public class SitecoreInitialiseSubCommand: InitialiseSubCommand
    {
        private readonly IEnumerable<ITopology> _topologies;

        public SitecoreInitialiseSubCommand(
            IEnumerable<ITopology> topologies,
            ICommandHandler<InitialiseProject> initialiseProjectCommandHandler) : base(initialiseProjectCommandHandler)
        {
            _topologies = topologies;
        }

        protected override string Name => "sitecore";
        protected override string Description => "Initialise a Sitecore project.";

        protected override void HydrateCommand(Command command)
        {
            command.Handler = CommandHandler.Create((SitecoreInitialise si) =>DoInitialise(si));
        }

        private async Task DoInitialise(SitecoreInitialise si)
        {
            GetUserInput(si);

            var composeTemplate = new DockerComposeTemplate();
            
            if (!string.IsNullOrEmpty(si.DockerComposeTemplatePath) && !string.IsNullOrEmpty(si.TopologyName))
            {
                throw new MultipleDockerComposeTemplatesPassed();
            }

            if (!string.IsNullOrEmpty(si.TopologyName))
            {
                var topology = _topologies.Single(t => t.Name == si.TopologyName);
                composeTemplate.Contents = topology.DockerComposeTemplate;
                composeTemplate.FileName = topology.DockerComposeTemplateName;
            }
            else
            {
                throw new NoDockerComposeTemplatesPassed();
            }

            var licenseStream = File.OpenRead(si.LicensePath);
            var licenseMemoryStream = new MemoryStream();
            var licenseGZipStream = new GZipStream(licenseMemoryStream, CompressionLevel.Optimal, false);
            await licenseStream.CopyToAsync(licenseGZipStream);
            licenseGZipStream.Close();
            var sitecoreLicense = Convert.ToBase64String(licenseMemoryStream.ToArray());

            si.PrivateVariables = new Dictionary<string, string>
            {
                {"Sitecore.SqlSaPassword", NonceService.Generate()},
                {"Sitecore.TelerikEncryptionKey", NonceService.Generate()},
                {"Sitecore.License", sitecoreLicense},
                {"Sitecore.CMPort", "44001"},
                {"Sitecore.CDPort", "44002"},
                {"Sitecore.SqlPort", "44010"},
                {"Sitecore.SolrPort", "44011"}
            };
            
            si.PublicVariables = new Dictionary<string, string>
            {
                {"SqlDockerImage", $"{si.Registry}/sitecore-xp-sqldev:${si.SitecoreVersion}-windowsservercore-${si.WindowsServerCoreVersion}"},
                {"SolrDockerImage", $"{si.Registry}/sitecore-xp-solr:${si.SitecoreVersion}-nanoserver-${si.NanoServerVersion}"},
                {"XConnectDockerImage", $"{si.Registry}/sitecore-xp-xconnect:${si.SitecoreVersion}-windowsservercore-${si.WindowsServerCoreVersion}"},
                {"XConnectAutomationEngineImage", $"{si.Registry}/sitecore-xp-xconnect-automationengine:${si.SitecoreVersion}-windowsservercore-${si.WindowsServerCoreVersion}"},
                {"XConnectIndexWorkerImage", $"{si.Registry}/sitecore-xp-xconnect-indexworker:${si.SitecoreVersion}-windowsservercore-${si.WindowsServerCoreVersion}"},
                {"XConnectProcessingEngineImage", $"{si.Registry}/sitecore-xp-xconnect-processingengine:${si.SitecoreVersion}-windowsservercore-${si.WindowsServerCoreVersion}"},
                {"CDImage", $"{si.Registry}/sitecore-xp-cd:${si.SitecoreVersion}-windowsservercore-${si.WindowsServerCoreVersion}"},
                {"CMImage", $"{si.Registry}/sitecore-xp-standalone:${si.SitecoreVersion}-windowsservercore-${si.WindowsServerCoreVersion}"},
                {"VisualStudio.RemoteDebugger", @"C:\Program Files\Microsoft Visual Studio 16.0\Common7\IDE\Remote Debugger"},
            };
            
            await InitialiseProjectCommandHandler.Handle(si);
        }

        private void GetUserInput(SitecoreInitialise sitecoreInitialise)
        {
            sitecoreInitialise.Name = sitecoreInitialise.Name.GetUserInput("Project Name:");
            sitecoreInitialise.SourceCodePath = sitecoreInitialise.SourceCodePath.GetUserInput("Source code path:");
            sitecoreInitialise.WorkingPath = sitecoreInitialise.WorkingPath.GetUserInput("Working path:");

            if (string.IsNullOrEmpty(sitecoreInitialise.TopologyName))
            {
                foreach (var topology in _topologies)
                {
                    Console.WriteLine(topology.Name);
                }
            }

            sitecoreInitialise.TopologyName = sitecoreInitialise.TopologyName.GetUserInput("Choose topology:");
            sitecoreInitialise.LicensePath = sitecoreInitialise.LicensePath.GetUserInput("license path:");
        }
    }
}
