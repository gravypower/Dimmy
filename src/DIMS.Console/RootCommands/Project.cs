using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using DIMS.Engine;
using DIMS.Engine.Commands;
using DIMS.Engine.Commands.DockerCompose;

namespace DIMS.CLI.RootCommands
{
    internal class Project : Command
    {
        private readonly ICommandHandler<StopProject> _stopProjectCommandHandler;
        private readonly ICommandHandler<StartProject> _startProjectCommandHandler;
        private readonly ICommandHandler<GenerateComposeYaml> _generateComposeYamlCommandHandler;

        public Project(
            ICommandHandler<StopProject> stopProjectCommandHandler,
            ICommandHandler<StartProject> startProjectCommandHandler,
            ICommandHandler<GenerateComposeYaml> generateComposeYamlCommandHandler) : base("project")
        {
            _stopProjectCommandHandler = stopProjectCommandHandler;
            _startProjectCommandHandler = startProjectCommandHandler;
            _generateComposeYamlCommandHandler = generateComposeYamlCommandHandler;
            AddAlias("project");

            AddGenerateComposeYamlSubCommand();
            AddStartProjectSubCommand();
            AddStopProjectSubCommand();
        }

        private void AddGenerateComposeYamlSubCommand()
        {
            var projectListCommand = new Command("GenerateComposeYaml")
            {
                new Option<string>("--license-path", "Path to Sitecore license file"),
                new Option<string>("--project-folder", "Project Folder"),
                new Option<string>("--project-name", "Project Name")
            };

            projectListCommand.Handler = CommandHandler
                .Create<string, string, string>((licensePath, projectFolder, projectName) =>
            {
                var generateComposeYaml = new GenerateComposeYaml
                {
                    Topology = new XpTopology(),
                    ProjectFolder = projectFolder,
                    LicenseStream = File.OpenRead(licensePath),
                    ProjectName = projectName
                };

                _generateComposeYamlCommandHandler.Handle(generateComposeYaml);
            });

            Add(projectListCommand);
        }

        private void AddStartProjectSubCommand()
        {
            var startProjectCommand = new Command("StartProject")
            {
                new Option<string>("--project-folder", "Project Folder")
            };

            startProjectCommand.Handler = CommandHandler
                .Create<string>(projectFolder =>
                {
                    var startProject = new StartProject
                    {
                        ProjectFolder = projectFolder
                    };

                    _startProjectCommandHandler.Handle(startProject);
                });

            Add(startProjectCommand);
        }

        private void AddStopProjectSubCommand()
        {
            var stopProjectCommand = new Command("StopProject")
            {
                new Option<Guid>("--project-id", "Project Id")
            };

            stopProjectCommand.Handler = CommandHandler
                .Create<Guid>(projectId =>
                {
                    var stopProject = new StopProject
                    {
                        ProjectId = projectId
                    };

                    _stopProjectCommandHandler.Handle(stopProject);
                });

            Add(stopProjectCommand);
        }
    }
}
