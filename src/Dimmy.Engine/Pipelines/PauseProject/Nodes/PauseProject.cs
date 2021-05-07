using System;
using System.Linq;
using System.Threading.Tasks;
using CliWrap;
using Dimmy.Engine.Pipelines.StopProject;
using Dimmy.Engine.Services.Projects;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Pipelines.PauseProject.Nodes
{
    public class PauseProject : Node<IPauseProjectContext>
    {


        public PauseProject()
        {

        }
        public override void DoExecute(IPauseProjectContext input)
        {
            using var stdOut = Console.OpenStandardOutput();
            using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker-compose")
                          .WithArguments(new[] {
                              "stop",
                          })
                          .WithWorkingDirectory(input.WorkingPath)
                      | (stdOut, stdErr);
            
            Task.WaitAll(cmd.ExecuteAsync());
            
        }
    }
}