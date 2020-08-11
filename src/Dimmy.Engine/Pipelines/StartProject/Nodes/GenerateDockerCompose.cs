using System.IO;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class GenerateDockerCompose:Node<StartProjectContext>
    {
        private readonly ICommandHandler<GenerateComposeYaml> _generateComposeYamlCommandHandler;

        public GenerateDockerCompose(ICommandHandler<GenerateComposeYaml> generateComposeYamlCommandHandler)
        {
            _generateComposeYamlCommandHandler = generateComposeYamlCommandHandler;
        }
        public override void DoExecute(StartProjectContext input)
        {
            var workingDockerCompose = Path.Combine(input.WorkingPath, "docker-compose.yml");
            if (File.Exists(workingDockerCompose)) File.Delete(workingDockerCompose);

            _generateComposeYamlCommandHandler.Handle(new GenerateComposeYaml
            {
                WorkingPath = input.WorkingPath
            });
        }
    }
}