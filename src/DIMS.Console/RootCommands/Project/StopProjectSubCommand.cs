using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using DIMS.Engine.Commands;
using DIMS.Engine.Commands.Docker;

namespace DIMS.CLI.RootCommands.Project
{
    public class StopProjectSubCommand:IProjectSubCommand
    {
        private readonly ICommandHandler<StopProject> _stopProjectCommandHandler;

        public StopProjectSubCommand(ICommandHandler<StopProject> stopProjectCommandHandler)
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
