using System.IO;
using Dimmy.Engine.Pipelines.GenerateComposeYaml;
using Dimmy.Engine.Services;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class GenerateDockerCompose:Node<IStartProjectContext>
    {
        public override int Order => -1;

        private readonly IDockerComposeParser _dockerComposeParser;
        private readonly Pipeline<Node<IGenerateComposeYamlContext>, IGenerateComposeYamlContext> _generateComposeYamlPipeline;


        public GenerateDockerCompose(
            IDockerComposeParser dockerComposeParser,
            Pipeline<Node<IGenerateComposeYamlContext>, IGenerateComposeYamlContext> generateComposeYamlPipeline)
        {
            _dockerComposeParser = dockerComposeParser;
            _generateComposeYamlPipeline = generateComposeYamlPipeline;
        }
        public override void DoExecute(IStartProjectContext input)
        {
            var workingDockerCompose = Path.Combine(input.WorkingPath, "docker-compose.yml");
            if (File.Exists(workingDockerCompose)) File.Delete(workingDockerCompose);

            
            _generateComposeYamlPipeline.Execute(new GenerateComposeYamlContext
            {
                Project = input.Project,
                ProjectInstance = input.ProjectInstance,
                WorkingPath = input.WorkingPath
            });
            
            
            if (!File.Exists(workingDockerCompose)) throw new DockerComposeFileNotFound();


            var dockerComposeFileString = File.ReadAllText(workingDockerCompose);
            input.DockerComposeFileConfig = _dockerComposeParser.ParseDockerComposeString(dockerComposeFileString);
        }
    }
}