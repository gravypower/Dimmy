using System;
using System.IO;
using Octostache;

namespace Dimmy.Sitecore.Plugin
{
    public class XpTopology:ITopology
    {

        public string Name => "XP";

        public string DockerComposeTemplateName => "docker-compose.xp.yml.template";
        public string DockerComposeTemplate { get; }
        
        public VariableDictionary VariableDictionary { get; } = new VariableDictionary();


        public XpTopology()
        {
            using var stream = GetType().Assembly.GetManifestResourceStream(DockerComposeTemplateName);
            
            if (stream == null)
            {
                throw new InvalidOperationException("Could not load manifest resource stream.");
            }
            using var reader = new StreamReader(stream);

            DockerComposeTemplate = reader.ReadToEnd();


            VariableDictionary.Set("SqlDockerImage", "ddcontainers.azurecr.io/sitecore-xp-sqldev:latest");
            VariableDictionary.Set("SolrDockerImage", "ddcontainers.azurecr.io/sitecore-xp-solr:latest");
            VariableDictionary.Set("XConnectDockerImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect:latest");
            VariableDictionary.Set("XConnectAutomationEngineImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect-automationengine:latest");
            VariableDictionary.Set("XConnectIndexWorkerImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect-indexworker:latest");
            VariableDictionary.Set("XConnectProcessingEngineImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect-processingengine:latest");
            VariableDictionary.Set("CDImage", "ddcontainers.azurecr.io/sitecore-xp-cd:latest");
            VariableDictionary.Set("CMImage", "ddcontainers.azurecr.io/sitecore-xp-standalone:latest");
            VariableDictionary.Set("HookName", "Hook");
        }
    }
}
