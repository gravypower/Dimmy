using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Dimmy.Engine.Models;
using Dimmy.Engine.Services;

namespace Dimmy.Engine.Commands.Docker
{
    public class GenerateComposeYamlCommandHandler:ICommandHandler<GenerateComposeYaml>
    {
        private readonly IProjectService _projectService;

        public GenerateComposeYamlCommandHandler(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public Task Handle(GenerateComposeYaml command)
        {
           return Task.Run(() => Run(command));
        }

        private void Run(GenerateComposeYaml command)
        {
            var licenseMemoryStream = new MemoryStream();
            var licenseGZipStream = new GZipStream(licenseMemoryStream, CompressionLevel.Optimal, false);
            command.LicenseStream.CopyTo(licenseGZipStream);
            licenseGZipStream.Close();
            var sitecoreLicense = Convert.ToBase64String(licenseMemoryStream.ToArray());

            ProjectYamlInstanceYaml contextProject;

            try
            {
                contextProject = _projectService.GetContextProject();
            }
            catch (ProjectContextFileNotFound e)
            {
                var variableDictionary = new Dictionary<string, string>();
                variableDictionary.Add("Sitecore.SqlSaPassword", NonceService.Generate());
                variableDictionary.Add("Sitecore.TelerikEncryptionKey", NonceService.Generate());
                variableDictionary.Add("Sitecore.License", sitecoreLicense);

                contextProject = _projectService.NewContextProject(
                    command.ProjectName,
                    command.ProjectFolder,
                    command.SourcePath,
                    "",
                    //command.Topology.DockerComposeTemplate,
                    variableDictionary);
            }
            
            //command.Topology.VariableDictionary.Set("Project.Name", command.ProjectName);
            //command.Topology.VariableDictionary.Set("Project.Id", $"{contextProject.Id:N}");
            //command.Topology.VariableDictionary.Set("Project.Folder", command.ProjectFolder);


            //foreach (var keyValuePair in contextProject.VariableDictionary)
            //{
            //    command.Topology.VariableDictionary.Set(keyValuePair.Key, keyValuePair.Value);
            //}

            
            //var dockerCompose = command.Topology.VariableDictionary.Evaluate(command.Topology.DockerComposeTemplate);

            //var dockerComposeFile = $"{command.ProjectFolder}\\docker-compose.yml";

            //File.WriteAllText(dockerComposeFile, dockerCompose);
        }
    }
}
