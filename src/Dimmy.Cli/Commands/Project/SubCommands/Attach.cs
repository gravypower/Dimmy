using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using Dimmy.Cli.Extensions;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;
using Dimmy.Engine.Services;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Attach : IProjectSubCommand
    {
        private readonly ICommandHandler<EnterPowershellSession> _enterPowerShellSessionCommandHandler;
        private readonly IProjectService _projectService;

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
                new Option<Guid>("--project-id",
                    "The Id of the Project you wish to attach to. Omit for context project"),
                new Option<string>("--role", "The role you want to connect to. Omit to pick"),
                new Option<bool>("--no-exit", "Don't exist host PowerShell session after exiting from container")
            };

            command.Handler = CommandHandler.Create((AttachArgument arg) =>
            {
                if (arg.ProjectId == Guid.Empty) arg.ProjectId = _projectService.GetProject().Project.Id;

                var project = _projectService.RunningProjects().Single(p => p.Id == arg.ProjectId);

                if (string.IsNullOrEmpty(arg.Role))
                {
                    project.PrettyPrint();
                    Console.Write("Enter name of role:");

                    arg.Role = Console.ReadLine();
                }

                var projectRole = project.Services.Single(r => r.Name == arg.Role);

                _enterPowerShellSessionCommandHandler.Handle(new EnterPowershellSession
                {
                    ContainerId = projectRole.ContainerId,
                    ShellTitle = $"{project.Name}({projectRole.Name})",
                    NoExit = arg.NoExit
                });
            });

            return command;
        }
    }
}