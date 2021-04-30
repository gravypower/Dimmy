using System;
using System.CommandLine;
using System.IO;
using Dimmy.Engine.Pipelines;
using Dimmy.Engine.Pipelines.StopProject;
using Dimmy.Engine.Services.Projects;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Stop : ProjectSubCommand<StopArgument>
    {
        private readonly IProjectService _projectService;
        private readonly Pipeline<Node<IStopProjectContext>, IStopProjectContext> _stopProjectPipeline;


        public Stop(
            IProjectService projectService,
            Pipeline<Node<IStopProjectContext>, IStopProjectContext> stopProjectPipeline)
        {
            _projectService = projectService;
            _stopProjectPipeline = stopProjectPipeline;
        }

        public override Command BuildCommand()
        {
            var stopProjectCommand = new Command("stop")
            {
                new Option<string>("--working-path", "Working Path"),
                new Option<string>("--project-id", "Project Id")
            };
            
            return stopProjectCommand;
        }

        public  override void CommandAction(StopArgument arg)
        {
           
            if (string.IsNullOrEmpty(arg.WorkingPath))
                arg.WorkingPath = Path.GetFullPath(Environment.CurrentDirectory);

            _stopProjectPipeline.Execute(new StopProjectContext
            {
                WorkingPath = arg.WorkingPath
            });
        }
    }
}