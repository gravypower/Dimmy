using System.IO;
using System.Threading.Tasks;
using Dimmy.Engine.Services;
using Octostache;

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
            var (projectInstance, project) = _projectService.GetProject(command.WorkingPath);

            var variableDictionary = new VariableDictionary();
            variableDictionary.Set("Project.Name", projectInstance.Name);
            variableDictionary.Set("Project.Id", $"{projectInstance.Id:N}");
            variableDictionary.Set("Project.WorkingPath", projectInstance.WorkingPath);

            foreach (var keyValuePair in projectInstance.VariableDictionary)
            {
                variableDictionary.Set(keyValuePair.Key, keyValuePair.Value);
            }

            foreach (var keyValuePair in project.VariableDictionary)
            {
                variableDictionary.Set(keyValuePair.Key, keyValuePair.Value);
            }

            var dockerCompose = variableDictionary.Evaluate(projectInstance.ComposeTemplate);

            var dockerComposeFile = Path.Combine(command.WorkingPath, "docker-compose.yml");

            File.WriteAllText(dockerComposeFile, dockerCompose);
        }
    }
}
