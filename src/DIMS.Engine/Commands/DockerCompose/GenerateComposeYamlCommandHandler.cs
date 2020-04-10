using System;
using System.IO;
using System.IO.Compression;

namespace DIMS.Engine.Commands.DockerCompose
{
    public class GenerateComposeYamlCommandHandler:ICommandHandler<GenerateComposeYaml>
    {
        public void Handle(GenerateComposeYaml command)
        {
            var licenseMemoryStream = new MemoryStream();
            var licenseGZipStream = new GZipStream(licenseMemoryStream, CompressionLevel.Optimal, false);
            command.LicenseStream.CopyTo(licenseGZipStream);
            licenseGZipStream.Close();
            var sitecoreLicense = Convert.ToBase64String(licenseMemoryStream.ToArray());

            command.Topology.VariableDictionary.Set("SqlSaPassword", command.SqlSaPassword);
            command.Topology.VariableDictionary.Set("SitecoreLicense", sitecoreLicense);
            command.Topology.VariableDictionary.Set("TelerikEncryptionKey", command.TelerikEncryptionKey);
            command.Topology.VariableDictionary.Set("ProjectFolder", command.ProjectFolder);
            command.Topology.VariableDictionary.Set("ProjectDataFolder", $@"{command.ProjectFolder}\data");

            var dockerCompose = command.Topology.VariableDictionary.Evaluate(command.Topology.DockerComposeTemplate);

            var dockerComposeFile = $"{command.ProjectFolder}\\docker-compose.yml";

            File.WriteAllText(dockerComposeFile, dockerCompose);
        }
    }
}
