using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using Octostache;

namespace DIMS.Engine
{
    public class Class1
    {
        public void test()
        {
            Logging.Enabled();

            var hosts = new Hosts().Discover();
            var host = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default");

            if(host == null)
                return;

            var dockerComposeFile = DockerComposeFile();

            var xp = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile(dockerComposeFile)
                .RemoveOrphans();

            var xpCompositeService = xp.Build();
            xpCompositeService.Start();

            foreach (var container in xpCompositeService.Containers.Where(c => c.Name == "cd" || c.Name == "cd"))
            {
                container.CopyTo(
                    @"C:\inetpub\wwwroot\bin",
                    @"C:\projects\DIMS\src\DIMS.DeploymentHook\bin\DIMS.DeploymentHook.dll");

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "C:\\windows\\system32\\windowspowershell\\v1.0\\powershell.exe",
                        Arguments = $"-NoLogo -NoExit -Command docker exec -it {container.Id} powershell",
                        RedirectStandardOutput = false,
                        UseShellExecute = true,
                        CreateNoWindow = false,
                    }
                };
                process.Start();
            }
            //
            xpCompositeService.Dispose();
        }

        private static string DockerComposeFile()
        {
            var clientFolder = @"C:\clients\SomeClient";
            var sqlSaPassword = "P@ssw0rd!123";

            var licenseFileStream = File.OpenRead(@"C:\license.xml");
            var licenseMemoryStream = new MemoryStream();
            var licenseGZipStream = new GZipStream(licenseMemoryStream, CompressionLevel.Optimal, false);
            licenseFileStream.CopyTo(licenseGZipStream);
            licenseGZipStream.Close();
            var sitecoreLicense = Convert.ToBase64String(licenseMemoryStream.ToArray());
            var telerikEncryptionKey = Guid.NewGuid();

            var dockerComposeTemplateFile = File.ReadAllText(@"C:\projects\DIMS\src\docker-compose.xp.yml.template");

            var variables = new VariableDictionary();
            variables.Set("SqlSaPassword", sqlSaPassword);
            variables.Set("SitecoreLicense", sitecoreLicense);
            variables.Set("TelerikEncryptionKey", $"{telerikEncryptionKey:N}");
            variables.Set("ClientFolder", $@"{clientFolder}");
            variables.Set("ClientDataFolder", $@"{clientFolder}\data");

            variables.Set("SqlDockerImage", "ddcontainers.azurecr.io/sitecore-xp-sqldev:latest");
            variables.Set("SolrDockerImage", "ddcontainers.azurecr.io/sitecore-xp-solr:latest");
            variables.Set("XConnectDockerImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect:latest");
            variables.Set("XConnectAutomationEngineImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect-automationengine:latest");
            variables.Set("XConnectIndexWorkerImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect-indexworker:latest");
            variables.Set("XConnectProcessingEngineImage", "ddcontainers.azurecr.io/sitecore-xp-xconnect-processingengine:latest");
            variables.Set("CDImage", "ddcontainers.azurecr.io/sitecore-xp-cd:latest");
            variables.Set("CMImage", "ddcontainers.azurecr.io/sitecore-xp-standalone:latest");
            variables.Set("HookName", "Hook");

            var dockerCompose = variables.Evaluate(dockerComposeTemplateFile);

            var dockerComposeFile = $"{clientFolder}\\docker-compose.xp.yml";

            File.WriteAllText(dockerComposeFile, dockerCompose);
            return dockerComposeFile;
        }
    }
}
