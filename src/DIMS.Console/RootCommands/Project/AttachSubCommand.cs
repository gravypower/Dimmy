using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using DIMS.CLI.Extensions;
using DIMS.Engine.Commands;
using DIMS.Engine.Commands.Docker;
using DIMS.Engine.Services;

namespace DIMS.CLI.RootCommands.Project
{
    public class AttachSubCommand:IProjectSubCommand
    {
        private readonly IProjectService _projectService;
        private readonly ICommandHandler<EnterPowershellSession> _enterPowershellSessionCommandHandler;

        public AttachSubCommand(
            IProjectService projectService,
            ICommandHandler<EnterPowershellSession> enterPowershellSessionCommandHandler)
        {
            _projectService = projectService;
            _enterPowershellSessionCommandHandler = enterPowershellSessionCommandHandler;
        }

        public Command GetCommand()
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

                    if (string.IsNullOrEmpty(role))
                    {
                        project.PrettyPrint();
                        Console.Write("Enter name of role:");

                        role = Console.ReadLine();
                    }

                    var projectRole = project.Roles.Single(r => r.Name == role);

                    _enterPowershellSessionCommandHandler.Handle(new EnterPowershellSession
                    {
                        ContainerId = projectRole.ContainerId,
                        ShellTitle = $"{project.Name}({projectRole.Name})"
                    });
                });

            return stopProjectCommand;
        }
    }
}
