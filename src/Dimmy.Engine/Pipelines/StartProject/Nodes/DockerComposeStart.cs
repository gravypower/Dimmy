using System;
using System.IO;
using Ductus.FluentDocker.AmbientContext;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Commands;
using Ductus.FluentDocker.Executors;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class DockerComposeStart : Node<IStartProjectContext>
    {
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
            var p = new ProcessManager();
            
            p.ErrorTextReceived += (sender, s) =>    
            {
                if(sender.ProcessIdentifier != nameof(Compose.ComposeUp))
                    return;
                
                if (!string.IsNullOrEmpty(s))
                    Console.Error.WriteAsync(s);
            };
            
            p.StandartTextReceived += (sender, s) => 
            {
                if(sender.ProcessIdentifier != nameof(Compose.ComposeUp))
                    return;
                
                if (!string.IsNullOrEmpty(s))
                    Console.Write(s);
            };
            
            using (ProcessManagerContext.UseProcessManager(p))
            {
               //compositeService.Start();
            }
        }
    }
}