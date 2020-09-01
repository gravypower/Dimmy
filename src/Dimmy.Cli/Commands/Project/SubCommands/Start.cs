using System;
using System.CommandLine;
using System.IO;
using Dimmy.Engine.Pipelines;
using Dimmy.Engine.Pipelines.StartProject;
using Dimmy.Engine.Pipelines.StartProject.Nodes;
using Dimmy.Engine.Services;
using Dimmy.Engine.Services.Projects;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Start : ProjectSubCommand<StartArgument>
    {
        private readonly IDockerComposeParser _dockerComposeParser;
        private readonly IProjectService _projectService;
        private readonly Pipeline<Node<IStartProjectContext>, IStartProjectContext> _startProjectPipeline;

        public Start(
            IDockerComposeParser dockerComposeParser,
            IProjectService projectService,
            Pipeline<Node<IStartProjectContext>, IStartProjectContext> startProjectPipeline)
        {
            _dockerComposeParser = dockerComposeParser;
            _projectService = projectService;
            _startProjectPipeline = startProjectPipeline;
        }

        public override Command BuildCommand()
        {
            var startProjectCommand = new Command("start")
            {
                new Option<string>("--working-path", "Working Path"),
                new Option<bool>("--generate-only", "Don't start the project only generate the docker compose file")
            };
            
            return startProjectCommand;
        }

        public  override void CommandAction(StartArgument arg)
        {
            if (string.IsNullOrEmpty(arg.WorkingPath))
                arg.WorkingPath = Path.GetFullPath(Environment.CurrentDirectory);
            
            var (projectInstance, project) = _projectService.GetProject(arg.WorkingPath);
            
            var dockerComposeFile = Path.Combine(arg.WorkingPath, "docker-compose.yml");
            if (!File.Exists(dockerComposeFile)) throw new DockerComposeFileNotFound();

            var dockerComposeFileString = File.ReadAllText(dockerComposeFile);
            var dockerComposeFileConfig = _dockerComposeParser.ParseDockerComposeString(dockerComposeFileString);
                
            _startProjectPipeline.Execute(new StartProjectContext
            {
                DockerComposeFileConfig = dockerComposeFileConfig, 
                GeneratOnly = arg.GenerateOnly,
                ProjectInstance = projectInstance,
                Project = project,
                WorkingPath = arg.WorkingPath
            });
        }
    }
}