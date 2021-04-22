using System.IO;
using System.Threading.Tasks;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class GenerateDockerCompose:Node<IStartProjectContext>
    {
        public override int Order => -1;
        public override async Task DoExecute(IStartProjectContext input)
        {
            var workingDockerComposeFile = Path.Combine(input.WorkingPath, "docker-compose.yml");
            if (File.Exists(workingDockerComposeFile)) File.Delete(workingDockerComposeFile);

            var dockerComposeFilePath = Path.Combine(
                input.ProjectInstance.SourceCodeLocation,
                input.Project.DockerComposeFileName);
            
            var dockerCompose = await File.ReadAllTextAsync(dockerComposeFilePath);
            await File.WriteAllTextAsync(workingDockerComposeFile, dockerCompose);

        }
    }
}