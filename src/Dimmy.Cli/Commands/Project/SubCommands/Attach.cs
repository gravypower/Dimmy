using System;
using System.CommandLine;
using System.Diagnostics;
using System.Linq;
using Dimmy.Cli.Extensions;
using Dimmy.Engine.Services.Projects;

namespace Dimmy.Cli.Commands.Project.SubCommands
{
    public class Attach : ProjectSubCommand<AttachArgument>
    {
        private readonly IProjectService _projectService;

        public Attach(IProjectService projectService)
        {
            _projectService = projectService;
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
            
            var shellTitle = $"{runningProject.Name}({projectRole.Name})";
            var setTitle = $"{{$host.ui.RawUI.WindowTitle = '{shellTitle}'}}";
            
            var noExit = arg.NoExit? "-NoExit" : "";
            
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments =
                        $"-NoLogo {noExit} -Command docker exec -it {projectRole.ContainerId} powershell -NoExit -Command {setTitle};",
                    RedirectStandardOutput = false,
                    UseShellExecute = true,
                    CreateNoWindow = false
                }
            };
            
            process.Start();
            
        }
    }
}