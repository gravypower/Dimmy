using System;
using System.Threading.Tasks;
using CliWrap;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class DockerComposeStart : Node<IStartProjectContext>
    {
        public DockerComposeStart()
        {
        }
        
        public override int Order => 999;

        public override void DoExecute(IStartProjectContext input)
        {
            if(input.GenerateOnly)
                return;

            using var stdOut = Console.OpenStandardOutput();
            using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker-compose")
                .WithArguments(new[] {
                    "up",
                    "--detach"
                })
                .WithWorkingDirectory(input.WorkingPath)
                      | (stdOut, stdErr);
            
            Task.WaitAll(cmd.ExecuteAsync());
        }
    }
}