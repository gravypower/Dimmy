using System;
using System.CommandLine;
using System.IO;
using Dimmy.Engine.Models.Yaml.DockerCompose;
using Dimmy.Engine.Pipelines;
using Dimmy.Engine.Pipelines.StartProject;
using Dimmy.Engine.Pipelines.StartProject.Nodes;
using Dimmy.Engine.Services.Projects;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Start : ProjectSubCommand<StartArgument>
    {
        private readonly IProjectService _projectService;
        private readonly Pipeline<Node<IStartProjectContext>, IStartProjectContext> _startProjectPipeline;

        public Start(
            IProjectService projectService,
            Pipeline<Node<IStartProjectContext>, IStartProjectContext> startProjectPipeline)
        {
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
            
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            var dockerCompose = 
                deserializer.Deserialize<DockerComposeYaml>(File.ReadAllText(dockerComposeFile));
            
            _startProjectPipeline.Execute(new StartProjectContext
            {
                DockerComposeYaml = dockerCompose,
                GeneratOnly = arg.GenerateOnly,
                ProjectInstance = projectInstance,
                Project = project,
                WorkingPath = arg.WorkingPath
            });
        }
    }
}