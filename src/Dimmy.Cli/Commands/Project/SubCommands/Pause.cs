using System;
using System.CommandLine;
using System.IO;
using Dimmy.Engine.Pipelines;
using Dimmy.Engine.Pipelines.PauseProject;
using Dimmy.Engine.Services.Projects;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Pause : ProjectSubCommand<PauseArgument>
    {
        private readonly IProjectService _projectService;
        private readonly Pipeline<Node<IPauseProjectContext>, IPauseProjectContext> _pauseProjectPipeline;

        public Pause(
            IProjectService projectService,
            Pipeline<Node<IPauseProjectContext>, IPauseProjectContext> pauseProjectPipeline)
        {
            _projectService = projectService;
            _pauseProjectPipeline = pauseProjectPipeline;
        }

        public override Command BuildCommand()
        {
            var stopProjectCommand = new Command("pause")
            {
                new Option<string>("--working-path", "Working Path"),
                new Option<string>("--project-id", "Project Id")
            };
            
            return stopProjectCommand;
        }

        public override void CommandAction(PauseArgument arg)
        {
            if (string.IsNullOrEmpty(arg.WorkingPath))
                arg.WorkingPath = Path.GetFullPath(Environment.CurrentDirectory);
            
            _pauseProjectPipeline.Execute(new PauseProjectContext
            {
                WorkingPath = arg.WorkingPath
            });
            
        }
    }
}