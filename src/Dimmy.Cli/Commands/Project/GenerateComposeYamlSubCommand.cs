using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;

namespace Dimmy.Cli.Commands.Project
{
    public class GenerateComposeYamlSubCommand:IProjectSubCommand
    {
        private readonly ICommandHandler<GenerateComposeYaml> _generateComposeYamlCommandHandler;

        public GenerateComposeYamlSubCommand(ICommandHandler<GenerateComposeYaml> generateComposeYamlCommandHandler)
        {
            _generateComposeYamlCommandHandler = generateComposeYamlCommandHandler;
        }

        public Command GetCommand()
        {
            var projectListCommand = new Command("generate-yaml")
            {
                new Option<string>("--project-folder", "Project Folder"),
                new Option<string>("--source-folder", "Source Folder"),
                new Option<string>("--project-name", "Project Name")
            };

            projectListCommand.Handler = CommandHandler
                .Create<string, string, string, string>(async (licensePath, projectFolder, sourceFolder, projectName) =>
                {
                    await NewMethod(projectFolder, licensePath, projectName, sourceFolder);
                });

            return projectListCommand;
        }

        private async Task NewMethod(string projectFolder, string licensePath, string projectName, string sourceFolder)
        {
            var generateComposeYaml = new GenerateComposeYaml
            {
                ProjectFolder = projectFolder,
                LicenseStream = File.OpenRead(licensePath),
                ProjectName = projectName,
                SourcePath = sourceFolder
            };

            await _generateComposeYamlCommandHandler.Handle(generateComposeYaml);
        }
    }
}
