using System;
using System.IO;
using System.IO.Compression;
using DIMS.Engine.Services;

namespace DIMS.Engine.Commands.DockerCompose
{
    public class GenerateComposeYamlCommandHandler:ICommandHandler<GenerateComposeYaml>
    {
        private readonly IProjectService _projectService;

        public GenerateComposeYamlCommandHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public void Handle(GenerateComposeYaml command)
        {
            var licenseMemoryStream = new MemoryStream();
            var licenseGZipStream = new GZipStream(licenseMemoryStream, CompressionLevel.Optimal, false);
            command.LicenseStream.CopyTo(licenseGZipStream);
            licenseGZipStream.Close();
            var sitecoreLicense = Convert.ToBase64String(licenseMemoryStream.ToArray());

            var projectId = _projectService.GetContextProjectId();

            command.Topology.VariableDictionary.Set("SqlSaPassword", command.SqlSaPassword);
            command.Topology.VariableDictionary.Set("Sitecore.License", sitecoreLicense);
            command.Topology.VariableDictionary.Set("TelerikEncryptionKey", command.TelerikEncryptionKey);
            command.Topology.VariableDictionary.Set("Project.Name", command.ProjectName);
            command.Topology.VariableDictionary.Set("Project.Id", $"{projectId:N}");
            command.Topology.VariableDictionary.Set("Project.Folder", command.ProjectFolder);

            var dockerCompose = command.Topology.VariableDictionary.Evaluate(command.Topology.DockerComposeTemplate);

            var dockerComposeFile = $"{command.ProjectFolder}\\docker-compose.yml";

            File.WriteAllText(dockerComposeFile, dockerCompose);
        }
    }
}
