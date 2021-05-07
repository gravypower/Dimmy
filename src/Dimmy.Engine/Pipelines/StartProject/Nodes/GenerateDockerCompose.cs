using System.IO;
using System.Threading.Tasks;
using Dimmy.Engine.Pipelines.GenerateDockerComposeFile;
using Dimmy.Engine.Services;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class GenerateDockerCompose:Node<IStartProjectContext>
    {
        private readonly Pipeline<Node<IGenerateDockerComposeFileContext>, IGenerateDockerComposeFileContext> _generateEnvironmentFilePipeline;
        private readonly IDockerComposeParser _dockerComposeParser;
        public override int Order => -1;
        
        public GenerateDockerCompose(
            Pipeline<Node<IGenerateDockerComposeFileContext>, IGenerateDockerComposeFileContext> generateEnvironmentFilePipeline,
            IDockerComposeParser dockerComposeParser)
        {
            _generateEnvironmentFilePipeline = generateEnvironmentFilePipeline;
            _dockerComposeParser = dockerComposeParser;
        }
        public override void DoExecute(IStartProjectContext input)
        {
            var workingDockerComposeFile = Path.Combine(input.WorkingPath, "docker-compose.yml");
            if (File.Exists(workingDockerComposeFile)) File.Delete(workingDockerComposeFile);

            var dockerComposeFilePath = Path.Combine(
                input.ProjectInstance.SourceCodeLocation,
                input.Project.DockerComposeTemplateFileName);
            
            _generateEnvironmentFilePipeline.Execute(new GenerateDockerComposeFileContext
                {
                    Project = input.Project,
                    ProjectInstance = input.ProjectInstance,
                    WorkingPath = input.WorkingPath
                });
            
            var dockerCompose = File.ReadAllText(dockerComposeFilePath);
            input.DockerComposeFileConfig = _dockerComposeParser.ParseDockerComposeString(dockerCompose);
        }
    }
}