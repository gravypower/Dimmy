using System;
using System.IO;
using Octostache;

namespace Dimmy.Engine
{
    public class XpTopology:ITopology
    {
        public string DockerComposeTemplate => File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/TopologyTemplates/docker-compose.xp.yml.template");
        public VariableDictionary VariableDictionary { get; } = new VariableDictionary();


        public XpTopology()
        {
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
