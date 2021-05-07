using System;
using System.Threading.Tasks;
using CliWrap;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class PullImages:Node<IStartProjectContext>
    {
        public PullImages()
        {
        }
        public override void DoExecute(IStartProjectContext input)
        {
            using var stdOut = Console.OpenStandardOutput();
            using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker-compose")
                          .WithArguments(new[] {
                              "pull"
                          })
                          .WithWorkingDirectory(input.WorkingPath)
                      | (stdOut, stdErr);
            
            Task.WaitAll(cmd.ExecuteAsync());
        }
    }
}