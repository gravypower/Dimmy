using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using Dimmy.Cli.Extensions;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;
using Dimmy.Engine.Services;

namespace Dimmy.Cli.Commands.Project
{
    public class Attach : IProjectSubCommand
    {
        private readonly IProjectService _projectService;
        private readonly ICommandHandler<EnterPowershellSession> _enterPowerShellSessionCommandHandler;


        public Attach(
            IProjectService projectService,
            ICommandHandler<EnterPowershellSession> enterPowerShellSessionCommandHandler)
        {
            _projectService = projectService;
            _enterPowerShellSessionCommandHandler = enterPowerShellSessionCommandHandler;
        }

        public Command GetCommand()
        {
            var command = new Command("attach", "Attach to a running container in a project.")
            {
                new Option<Guid>("--project-id", "The Id of the Project you wish to attach to. Omit for context project"),
                new Option<string>("--role", "The role you want to connect to. Omit to pick"),
                new Option<bool>("--no-exit", "Don't exist host PowerShell session after exiting from container")
            };

            command.Handler = CommandHandler
                .Create<Guid, string, bool>((projectId, role, noExit) =>
                {
                    
                    if (projectId == Guid.Empty)
                    {
                        projectId = _projectService.GetProject().Project.Id;
                    }

                    var project = _projectService.RunningProjects().Single(p => p.Id == projectId);

                    if (string.IsNullOrEmpty(role))
                    {
                        project.PrettyPrint();
                        Console.Write("Enter name of role:");

                        role = Console.ReadLine();
                    }

                    var projectRole = project.Services.Single(r => r.Name == role);

                    _enterPowerShellSessionCommandHandler.Handle(new EnterPowershellSession
                    {
                        ContainerId = projectRole.ContainerId,
                        ShellTitle = $"{project.Name}({projectRole.Name})",
                        NoExit = noExit
                    });
                });

            return command;
        }
    }
}
