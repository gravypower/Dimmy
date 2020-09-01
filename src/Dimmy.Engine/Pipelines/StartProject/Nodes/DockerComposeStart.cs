using System;
using System.IO;
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

        public override void DoExecute(IStartProjectContext input)
        {
            if(input.GeneratOnly)
                return;

            var workingDockerCompose = Path.Combine(input.WorkingPath, "docker-compose.yml");
            var builder = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile(workingDockerCompose)
                .RemoveOrphans();

            var compositeService = builder.Build();
            var p = new DataReceived();
            
            p.ErrorDataReceived += (sender, s) =>    
            {
                if(s.ProcessIdentifier != nameof(Compose.ComposeUp))
                    return;
                
                if (!string.IsNullOrEmpty(s.Data))
                    Console.Error.WriteLine(s.Data);
            };
            
            p.OutputDataReceived += (sender, s) => 
            {
                if(s.ProcessIdentifier != nameof(Compose.ComposeUp))
                    return;
                
                if (!string.IsNullOrEmpty(s.Data))
                    Console.WriteLine(s.Data);
            };
            
            using (DataReceivedContext.UseProcessManager(p))
            {
               compositeService.Start();
            }
        }
    }
}