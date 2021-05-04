using System;
using System.Threading.Tasks;
using CliWrap;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class DockerComposeStart : Node<IStartProjectContext>
    {
        public DockerComposeStart()
        {
        }
        
        public override int Order => 999;

        public override async Task DoExecute(IStartProjectContext input)
        {
            if(input.GenerateOnly)
                return;

            await using var stdOut = Console.OpenStandardOutput();
            await using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker-compose")
                .WithArguments(new[] {
                    "up",
                    "--detach"
                })
                .WithWorkingDirectory(input.WorkingPath)
                      | (stdOut, stdErr);
            
            await cmd.ExecuteAsync();
        }
    }
}