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

namespace Dimmy.Sitecore.Plugin
{
    public class InitialiseSitecore: InitialiseSubCommand
    {
        private readonly IEnumerable<ITopology> _topologies;

        public InitialiseSitecore(
            IEnumerable<ITopology> topologies,
            ICommandHandler<InitialiseProject> initialiseProjectCommandHandler) : base(initialiseProjectCommandHandler)
        {
            _topologies = topologies;
        }

        protected override string Name => "sitecore";
        protected override string Description => "Initialise a Sitecore project.";
        
        protected override void HydrateCommand(Command command)
        {
            command.AddOption(new Option<string>("--license-path"));

            command.AddOption(new Option<string>("--topology-name"));

            command.Handler = CommandHandler.Create<string, string, string, string, string, string>(DoInitialise);
        }

        public async Task DoInitialise(string name, string sourceCodePath, string workingPath, string dockerComposeTemplate,  string licensePath, string topologyName)
        {
            name = name.GetUserInput("Project Name:");
            sourceCodePath = sourceCodePath.GetUserInput("Source code path:");
            workingPath = workingPath.GetUserInput("Working path:");

            if (string.IsNullOrEmpty(topologyName))
            {
                foreach (var topology in _topologies)
                {
                    Console.WriteLine(topology.Name);
                }
            }
            topologyName = topologyName.GetUserInput("Choose topology:");

            licensePath = licensePath.GetUserInput("license path:");

            var composeTemplate = new DockerComposeTemplate();


            if (!string.IsNullOrEmpty(dockerComposeTemplate) && !string.IsNullOrEmpty(topologyName))
            {
                throw new MultipleDockerComposeTemplatesPassed();
            }

            if (!string.IsNullOrEmpty(dockerComposeTemplate))
            {
                composeTemplate.Contents = File.ReadAllText(dockerComposeTemplate);
                composeTemplate.FileName = Path.GetFileName(dockerComposeTemplate);
            }
            else if (!string.IsNullOrEmpty(topologyName))
            {
                var topology = _topologies.Single(t => t.Name == topologyName);
                composeTemplate.Contents = topology.DockerComposeTemplate;
                composeTemplate.FileName = topology.DockerComposeTemplateName;
            }
            else
            {
                throw new NoDockerComposeTemplatesPassed();
            }

            var licenseStream = File.OpenRead(licensePath);
            var licenseMemoryStream = new MemoryStream();
            var licenseGZipStream = new GZipStream(licenseMemoryStream, CompressionLevel.Optimal, false);
            licenseStream.CopyTo(licenseGZipStream);
            licenseGZipStream.Close();
            var sitecoreLicense = Convert.ToBase64String(licenseMemoryStream.ToArray());

            var privateVariables = new Dictionary<string, string>
            {
                {"Sitecore.SqlSaPassword", NonceService.Generate()},
                {"Sitecore.TelerikEncryptionKey", NonceService.Generate()},
                {"Sitecore.License", sitecoreLicense},
                {"Sitecore.CMPort", "44001"},
                {"Sitecore.CDPort", "44002"},
                {"Sitecore.SqlPort", "44010"},
                {"Sitecore.SolrPort", "44011"}
            };

            var publicVariables = new Dictionary<string, string>
            {
                {"SqlDockerImage", "ddcontainers.azurecr.io/sitecore-xp-sqldev:latest"},
                {"SolrDockerImage", "ddcontainers.azurecr.io/sitecore-xp-solr:latest"},
                {"XConnectDockerImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect:latest"},
                {"XConnectAutomationEngineImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect-automationengine:latest"},
                {"XConnectIndexWorkerImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect-indexworker:latest"},
                {"XConnectProcessingEngineImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect-processingengine:latest"},
                {"CDImage", "ddcontainers.azurecr.io/sitecore-xp-cd:latest"},
                {"CMImage", "ddcontainers.azurecr.io/sitecore-xp-standalone:latest"},
                {"HookName", "Hook"},
            };

            var initialiseProject = new InitialiseProject
            {
                Name = name,
                WorkingPath = workingPath,
                SourceCodePath = sourceCodePath,
                DockerComposeTemplate = composeTemplate,
                PublicVariables = publicVariables,
                PrivateVariables = privateVariables
            };

            await InitialiseProjectCommandHandler.Handle(initialiseProject);

        }
    }

    public class NoDockerComposeTemplatesPassed : Exception
    {
    }

    public class MultipleDockerComposeTemplatesPassed : Exception
    {
    }
}
