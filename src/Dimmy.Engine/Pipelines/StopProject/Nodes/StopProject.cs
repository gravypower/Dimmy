using System;
using System.Threading.Tasks;
using CliWrap;

namespace Dimmy.Engine.Pipelines.StopProject.Nodes
{
    public class StopProject : Node<IStopProjectContext>
    {
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