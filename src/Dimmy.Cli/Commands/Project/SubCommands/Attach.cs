using System;
using System.CommandLine;
using System.Linq;
using System.Runtime.InteropServices;
using Dimmy.Cli.Extensions;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Commands.Docker;
using Dimmy.Engine.Services.Projects;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Attach : ProjectSubCommand<AttachArgument>
    {
        private readonly ICommandHandler<EnterPowershellSession> _enterPowerShellSessionCommandHandler;
        private readonly IProjectService _projectService;
        private readonly ICommandHandler<EnterBashSession> _enterBashSessionCommandHandler;

        public Attach(
            IProjectService projectService,
            ICommandHandler<EnterBashSession> enterBashSessionCommandHandler,
            ICommandHandler<EnterPowershellSession> enterPowerShellSessionCommandHandler)
        {
            _projectService = projectService;
            _enterBashSessionCommandHandler = enterBashSessionCommandHandler;
            _enterPowerShellSessionCommandHandler = enterPowerShellSessionCommandHandler;
        }

        public override Command BuildCommand()
        {
            var command = new Command("attach", "Attach to a running container in a project.")
            {
                new Option<Guid>("--project-id", "The Id of the Project you wish to attach to. Omit for context project"),
                new Option<string>("--working-path", "Working Path"),
                new Option<string>("--role", "The role you want to connect to. Omit to pick"),
                new Option<bool>("--no-exit", "Don't exist host PowerShell session after exiting from container")
            };

            return command;
        }

        public override void CommandAction(AttachArgument arg)
        {
            var runningProject = _projectService.ResolveRunningProject(arg);

            if (string.IsNullOrEmpty(arg.Role))
            {
                runningProject.PrettyPrint();
                Console.Write("Enter name of role:");

                arg.Role = Console.ReadLine();
            }

            var projectRole = runningProject.Services.Single(r => r.Name == arg.Role);
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                _enterBashSessionCommandHandler.Handle(new EnterBashSession
                {
                    ContainerId = projectRole.ContainerId
                });
            }
            else
            {
                _enterPowerShellSessionCommandHandler.Handle(new EnterPowershellSession
                {
                    ContainerId = projectRole.ContainerId,
                    ShellTitle = $"{runningProject.Name}({projectRole.Name})",
                    NoExit = arg.NoExit
                });
            }
        }
    }
}