using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Dimmy.Engine;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;

namespace Dimmy.Cli.RootCommands.Project
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
                new Option<string>("--license-path", "Path to Sitecore license file"),
                new Option<string>("--project-folder", "Project Folder"),
                new Option<string>("--project-name", "Project Name")
            };

            projectListCommand.Handler = CommandHandler
                .Create<string, string, string>(async (licensePath, projectFolder, projectName) =>
                {
                    var generateComposeYaml = new GenerateComposeYaml
                    {
                        Topology = new XpTopology(),
                        ProjectFolder = projectFolder,
                        LicenseStream = File.OpenRead(licensePath),
                        ProjectName = projectName
                    };

                    await _generateComposeYamlCommandHandler.Handle(generateComposeYaml);
                });

            return projectListCommand;
        }
    }
}
