using System.IO;
using System.Threading.Tasks;
using Ductus.FluentDocker.Model.Compose;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class CreateBindMountFolders : Node<IStartProjectContext>
    {
        public override void DoExecute(IStartProjectContext input)
        {
            foreach (var service in input.DockerComposeFileConfig.ServiceDefinitions)
            {
                if(service.Volumes == null)
                    continue;
                
                foreach (var volume in service.Volumes)
                {
                    var hostFolderName = string.Empty;
                    switch (volume)
                    {
                        case ShortServiceVolumeDefinition v:
                        {
                            var volumeParts = v.Entry.Split(':');
                            hostFolderName = volumeParts[0];
                            break;
                        }
                        case LongServiceVolumeDefinition v:
                        {
                            if (v.Type == VolumeType.Bind)
                            {
                                hostFolderName = v.Source;
                            }
                            break;
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