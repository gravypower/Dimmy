using System;
using System.Linq;
using System.Threading.Tasks;
using CliWrap;
using Dimmy.Engine.Services.Projects;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Pipelines.StopProject.Nodes
{
    public class StopProject : Node<IStopProjectContext>
    {
        private readonly IHostService _hostService;
        private readonly IProjectService _projectService;

        public StopProject(
            IHostService hostService,
            IProjectService projectService)
        {
            _hostService = hostService;
            _projectService = projectService;
        }
        public override void DoExecute(IStopProjectContext input)
        {
            using var stdOut = Console.OpenStandardOutput();
            using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker-compose")
                          .WithArguments(new[] {
                              "down",
                          })
                          .WithWorkingDirectory(input.WorkingPath)
                      | (stdOut, stdErr);
            
            Task.WaitAll(cmd.ExecuteAsync());
        }
    }
}