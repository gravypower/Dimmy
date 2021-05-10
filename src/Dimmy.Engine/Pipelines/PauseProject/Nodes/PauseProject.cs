using System;
using System.Threading.Tasks;
using CliWrap;
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