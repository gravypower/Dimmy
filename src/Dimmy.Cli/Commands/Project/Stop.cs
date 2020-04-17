using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;

namespace Dimmy.Cli.Commands.Project
{
    public class Stop:IProjectSubCommand
    {
        private readonly ICommandHandler<StopProject> _stopProjectCommandHandler;

        public Stop(ICommandHandler<StopProject> stopProjectCommandHandler)
        {
            _stopProjectCommandHandler = stopProjectCommandHandler;
        }

        public Command GetCommand()
        {
            var stopProjectCommand = new Command("stop")
            {
                new Option<Guid>("--project-id", "Project Id")
            };

            stopProjectCommand.Handler = CommandHandler
                .Create<Guid>(async projectId =>
                {
                    var stopProject = new StopProject
                    {
                        ProjectId = projectId
                    };

                    await _stopProjectCommandHandler.Handle(stopProject);
                });
            return stopProjectCommand;
        }
    }
}
