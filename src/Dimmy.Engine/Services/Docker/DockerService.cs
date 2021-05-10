using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CliWrap;
using Dimmy.Engine.Models.Docker;
using Newtonsoft.Json;

namespace Dimmy.Engine.Services.Docker
{
    public class DockerService:IDockerService
    {
        public void StartContainer(string containerId)
        {
            using var stdOut = Console.OpenStandardOutput();
            using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker")
                          .WithArguments(new[] {
                              "start",
                              containerId
                          })
                      | (stdOut, stdErr);
            
            Task.WaitAll(cmd.ExecuteAsync());
        }

        public void StopContainer(string containerId)
        {
            using var stdOut = Console.OpenStandardOutput();
            using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker")
                          .WithArguments(new[] {
                              "stop",
                              containerId
                          })
                      | (stdOut, stdErr);
            
            Task.WaitAll(cmd.ExecuteAsync());
        }

        public void RunPowershellInContainer(string containerId, string powershellScriptPath)
        {
            using var stdOut = Console.OpenStandardOutput();
            using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker")
                          .WithArguments(new[] {
                              "exec",
                              containerId,
                              "powershell",
                              "-command",
                              $"{powershellScriptPath}"
                          })
                      | (stdOut, stdErr);
            
            Task.WaitAll(cmd.ExecuteAsync());
        }

        public IEnumerable<ContainerLs> RunDockerContainerLsAll()
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
            
            Task.WaitAll(cmd.ExecuteAsync());

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

        public ImageInspect RunImageInspect(string imageId)
        {
            var stdOut = new StringBuilder();
            var stdErr = new StringBuilder();
            
            var cmd = Cli.Wrap("docker")
                          .WithArguments(new[] {
                              "container",
                              "ls",
                              "--format='{{json .}}'",
                              "--all"
                          })
                      | (stdOut, stdErr);
            
            Task.WaitAll(cmd.ExecuteAsync());

            var output = stdOut.ToString();

            return JsonConvert.DeserializeObject<ImageInspect>(output);;
        }
    }
}