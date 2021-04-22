using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.EventStream;
using Ductus.FluentDocker.AmbientContext;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Commands;
using Ductus.FluentDocker.Executors.ProcessDataReceived;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class DockerComposeStart : Node<IStartProjectContext>
    {
        private readonly IHostService _docker;

        public DockerComposeStart(IHostService docker )
        {
            _docker = docker;
        }
        
        public override int Order => 999;

        public override async Task DoExecute(IStartProjectContext input)
        {
            if(input.GenerateOnly)
                return;

            await using var stdOut = Console.OpenStandardOutput();
            await using var stdErr = Console.OpenStandardError();

            var cmd = Cli.Wrap("docker")
                .WithArguments(new[] {
                    "compose", 
                    "up",
                    "--detach"
                })
                .WithWorkingDirectory(input.WorkingPath)
                      | (stdOut, stdErr);
            
            await cmd.ExecuteAsync();

            // var workingDockerCompose = Path.Combine(input.WorkingPath, "docker-compose.yml");
            // var builder = new Builder()
            //     .UseContainer()
            //     .UseCompose()
            //     .FromFile(workingDockerCompose)
            //     .RemoveOrphans();
            //
            // var compositeService = builder.Build();
            // var p = new DataReceived();
            //
            // p.ErrorDataReceived += (sender, s) =>    
            // {
            //     if(s.ProcessIdentifier != nameof(Compose.ComposeUp))
            //         return;
            //     
            //     if (!string.IsNullOrEmpty(s.Data))
            //         Console.Error.Write(s.Data);
            // };
            //
            // p.OutputDataReceived += (sender, s) => 
            // {
            //     if(s.ProcessIdentifier != nameof(Compose.ComposeUp))
            //         return;
            //     
            //     if (!string.IsNullOrEmpty(s.Data))
            //         Console.Write(s.Data);
            // };
            //
            // using (DataReceivedContext.UseProcessManager(p))
            // {
            //    compositeService.Start();
            // }
        }
    }
}