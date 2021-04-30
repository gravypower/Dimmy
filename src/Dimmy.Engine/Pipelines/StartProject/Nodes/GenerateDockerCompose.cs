using System.IO;
using System.Threading.Tasks;
using Dimmy.Engine.Services;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class GenerateDockerCompose:Node<IStartProjectContext>
    {
        private readonly IDockerComposeParser _dockerComposeParser;
        public override int Order => -1;
        
        public GenerateDockerCompose(IDockerComposeParser dockerComposeParser)
        {
            _dockerComposeParser = dockerComposeParser;
        }
        public override async Task DoExecute(IStartProjectContext input)
        {
            var workingDockerComposeFile = Path.Combine(input.WorkingPath, "docker-compose.yml");
            if (File.Exists(workingDockerComposeFile)) File.Delete(workingDockerComposeFile);

            var dockerComposeFilePath = Path.Combine(
                input.ProjectInstance.SourceCodeLocation,
                input.Project.DockerComposeFileName);
            
            var dockerCompose = await File.ReadAllTextAsync(dockerComposeFilePath);
            await File.WriteAllTextAsync(workingDockerComposeFile, dockerCompose);
            
            input.DockerComposeFileConfig = _dockerComposeParser.ParseDockerComposeString(dockerCompose);
        }
    }
}