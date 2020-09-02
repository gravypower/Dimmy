using System.IO;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;
using Dimmy.Engine.Services;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class GenerateDockerCompose:Node<IStartProjectContext>
    {
        public override int Order => -1;

        private readonly IDockerComposeParser _dockerComposeParser;
        private readonly ICommandHandler<GenerateComposeYaml> _generateComposeYamlCommandHandler;

        public GenerateDockerCompose(
            IDockerComposeParser dockerComposeParser,
            ICommandHandler<GenerateComposeYaml> generateComposeYamlCommandHandler)
        {
            _dockerComposeParser = dockerComposeParser;
            _generateComposeYamlCommandHandler = generateComposeYamlCommandHandler;
        }
        public override void DoExecute(IStartProjectContext input)
        {
            var workingDockerCompose = Path.Combine(input.WorkingPath, "docker-compose.yml");
            if (File.Exists(workingDockerCompose)) File.Delete(workingDockerCompose);

            _generateComposeYamlCommandHandler.Handle(new GenerateComposeYaml
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