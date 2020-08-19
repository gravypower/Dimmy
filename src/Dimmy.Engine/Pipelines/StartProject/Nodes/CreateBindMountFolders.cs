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
            
            foreach (var dockerComposeService in input.DockerComposeYaml.Services)
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

                    var hostFolderPath = Path.Combine(input.WorkingPath, hostFolderName);
                    var exists = Directory.Exists(hostFolderPath);

                    if (!exists)
                        Directory.CreateDirectory(hostFolderPath);
                }
            }
        }
    }
}