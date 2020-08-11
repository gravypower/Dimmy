using System;
using Ductus.FluentDocker.Builders;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class DockerComposeStart : Node<IStartProjectContext>
    {
        public override void DoExecute(IStartProjectContext input)
        {
            var command = input.Command;
            var builder = new Builder()
                .UseContainer()
                .UseCompose()
                .FromFile(command.DockerComposeFilePath)
                .RemoveOrphans();

            var compositeService = builder.Build();

            compositeService.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                    Console.Out.WriteLineAsync(args.Data);
            };
            
            compositeService.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                    Console.Error.WriteLineAsync(args.Data);
            };

            compositeService.Start();
        }
    }
}