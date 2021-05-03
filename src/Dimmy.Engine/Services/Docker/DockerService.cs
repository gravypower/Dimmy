using System;
using System.Threading.Tasks;
using CliWrap;

namespace Dimmy.Engine.Services.Docker
{
    public class DockerService:IDockerService
    {
        public async Task StartContainer(string containerId)
        {
            await using var stdOut = Console.OpenStandardOutput();
            await using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker")
                          .WithArguments(new[] {
                              "start",
                              containerId
                          })
                      | (stdOut, stdErr);
            
            await cmd.ExecuteAsync();
        }

        public async Task StopContainer(string containerId)
        {
            await using var stdOut = Console.OpenStandardOutput();
            await using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker")
                          .WithArguments(new[] {
                              "stop",
                              containerId
                          })
                      | (stdOut, stdErr);
            
            await cmd.ExecuteAsync();
        }

        public async Task RunPowershellInContainer(string containerId, string powershellScriptPath)
        {
            await using var stdOut = Console.OpenStandardOutput();
            await using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker")
                          .WithArguments(new[] {
                              "exec",
                              containerId,
                              "powershell",
                              "-command",
                              $"{powershellScriptPath}"
                          })
                      | (stdOut, stdErr);
            
            await cmd.ExecuteAsync();
        }
    }
}