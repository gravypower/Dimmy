using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using Newtonsoft.Json;

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

        public async Task<IList<ContainerLs>> RunDockerContainerLsAll()
        {
            var stdOut = new StringBuilder();
            var stdErr = new StringBuilder();


            var cmd = Cli.Wrap("docker")
                          .WithArguments(new[] {
                              "container",
                              "ls",
                              "--format='{{json .}}.'",
                              "--all"
                          })
                      | (stdOut, stdErr);
            
            await cmd.ExecuteAsync();

            var output = stdOut.ToString();

            var resultStrings = output.Split("}.");
            var results = new List<ContainerLs>();
            foreach (var result in resultStrings)
            {
                var t = result
                    .Replace("\n", string.Empty)
                    .Replace("'", string.Empty);
                
                if(string.IsNullOrEmpty(t))
                    continue;
                
                var r = JsonConvert.DeserializeObject<ContainerLs>($"{t}}}");
                results.Add(r);
            }

            return results;
        }
    }
}