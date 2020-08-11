using System.Collections.Generic;
using System.IO;
using Dimmy.Engine.Models.Yaml.DockerCompose;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class CreateBindMountFolders : Node<IStartProjectContext>
    {
        public override void DoExecute(IStartProjectContext input)
        {
            var dockerComposeFile = Path.Combine(input.WorkingPath, "docker-compose.yml");
            if (!File.Exists(dockerComposeFile)) throw new DockerComposeFileNotFound();
            
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            var dockerCompose = 
                deserializer.Deserialize<DockerComposeYaml>(File.ReadAllText(dockerComposeFile));
            
            foreach (var dockerComposeService in dockerCompose.Services)
            {
                if(dockerComposeService.Value.Volumes == null)
                    continue;
                
                foreach (var volume in dockerComposeService.Value.Volumes)
                {
                    var hostFolderName = string.Empty;
                    if (volume is string)
                    {
                        var volumeParts = volume.Split(':');
                        hostFolderName = volumeParts[0];
                    }
                    else if (volume is IDictionary<object, object>)
                    {
                        if (volume["type"] == "bind")
                        {
                            hostFolderName = volume["source"];
                        }
                    }

                    if(string.IsNullOrEmpty(hostFolderName))
                        continue;

                    var composeFilePath = Path.GetDirectoryName(input.WorkingPath);

                    var hostFolderPath = Path.Combine(composeFilePath, hostFolderName);
                    var exists = Directory.Exists(hostFolderPath);

                    if (!exists)
                        Directory.CreateDirectory(hostFolderPath);
                }
            }

        }
    }
}