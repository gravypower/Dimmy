using System;
using System.CommandLine;
using System.IO;
using Dimmy.Engine.Pipelines;
using Dimmy.Engine.Pipelines.StartProject;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Start : ProjectSubCommand<StartArgument>
    {
        private readonly Pipeline<Node<IStartProjectContext>, IStartProjectContext> _startProjectPipeline;

        public Start(Pipeline<Node<IStartProjectContext>, IStartProjectContext> startProjectPipeline)
        {
            _startProjectPipeline = startProjectPipeline;
        }

        public override Command GetCommand()
        {
            var startProjectCommand = new Command("start")
            {
                new Option<string>("--working-path", "Working Path"),
                new Option<bool>("--generate-only", "Don't start the project only generate the docker compose file")
            };
            
            return startProjectCommand;
        }

        protected override void CommandAction(StartArgument arg)
        {
            if (string.IsNullOrEmpty(arg.WorkingPath))
                arg.WorkingPath = Path.GetFullPath(Environment.CurrentDirectory);
            
            if (arg.GeneratOnly)
                return;
            
            _startProjectPipeline.Execute(new StartProjectContext
            {
                GeneratOnly = arg.GeneratOnly,
                WorkingPath = arg.WorkingPath
            });
        }
    }
}