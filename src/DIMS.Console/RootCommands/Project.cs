using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using DIMS.Engine;
using DIMS.Engine.Commands;
using DIMS.Engine.Commands.DockerCompose;
using DIMS.Engine.Services;

namespace DIMS.CLI.RootCommands
{
    internal class Project : Command
    {
        private readonly IProjectService _projectService;
        private readonly ICommandHandler<StopProject> _stopProjectCommandHandler;
        private readonly ICommandHandler<StartProject> _startProjectCommandHandler;
        private readonly ICommandHandler<GenerateComposeYaml> _generateComposeYamlCommandHandler;
        private readonly ICommandHandler<EnterPowershellSession> _enterPowershellSessionCommandHandler;

        public Project(
            IProjectService projectService,
            ICommandHandler<StopProject> stopProjectCommandHandler,
            ICommandHandler<StartProject> startProjectCommandHandler,
            ICommandHandler<GenerateComposeYaml> generateComposeYamlCommandHandler,
            ICommandHandler<EnterPowershellSession> enterPowershellSessionCommandHandler) : base("project")
        {
            _projectService = projectService;
            _stopProjectCommandHandler = stopProjectCommandHandler;
            _startProjectCommandHandler = startProjectCommandHandler;
            _generateComposeYamlCommandHandler = generateComposeYamlCommandHandler;
            _enterPowershellSessionCommandHandler = enterPowershellSessionCommandHandler;
            AddAlias("project");

            AddGenerateComposeYamlSubCommand();
            AddStartProjectSubCommand();
            AddStopProjectSubCommand();
            AddAttachSubCommand();
        }

        private void AddGenerateComposeYamlSubCommand()
        {
            var projectListCommand = new Command("generate-yaml")
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
            var startProjectCommand = new Command("start")
            {
                new Option<string>("--project-folder", "Project Folder")
            };

            startProjectCommand.Handler = CommandHandler
                .Create<string>(projectFolder =>
                {

                    if (string.IsNullOrEmpty(projectFolder))
                    {
                        projectFolder = ".";
                    }

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
            var stopProjectCommand = new Command("stop")
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

        private void AddAttachSubCommand()
        {
            var stopProjectCommand = new Command("attach")
            {
                new Option<Guid>("--project-id", "Project Id"),
                new Option<string>("--role", "role")
            };

            stopProjectCommand.Handler = CommandHandler
                .Create<Guid, string>((projectId, role) =>
                {
                    if (projectId == Guid.Empty)
                    {
                        projectId = _projectService.GetContextProjectId();
                    }


                    var project = _projectService.RunningProjects().Single(p => p.Id == projectId);

                    var projectRole = project.Roles.Single(r => r.Name == role);

                    _enterPowershellSessionCommandHandler.Handle(new EnterPowershellSession
                    {
                        ContainerId = projectRole.ContainerId,
                        ShellTitle = $"{project.Name}({projectRole.Name})"
                    });
                });

            Add(stopProjectCommand);
        }
    }
}
