using System;
using System.IO;
using System.Threading.Tasks;
using CliWrap;
using Dimmy.Engine.Services.Docker;

namespace Dimmy.Engine.Pipelines.CopyFileToContainer.Nodes
{
    public class CopyFileToContainer : Node<ICopyFileToContainerContext>
    {
        private readonly IDockerService _dockerService;

        public CopyFileToContainer(
            IDockerService dockerService)
        {
            _dockerService = dockerService;
        }
        public override void DoExecute(ICopyFileToContainerContext input)
        {
            _dockerService.StopContainer(input.ContainerId);
            
            using var stdOut = Console.OpenStandardOutput();
            using var stdErr = Console.OpenStandardError();

            foreach (var file in input.CopyFiles)
            {
                var fileName = Path.GetFileName(file.TargetFilePath);
                var cmd = Cli.Wrap("docker")
                              .WithArguments(new[]
                              {
                                  "cp",
                                  file.TargetFilePath,
                                  $"{input.ContainerId}:{file.DestinationFolder}\\{fileName}"
                              })
                              .WithWorkingDirectory(input.WorkingPath)
                          | (stdOut, stdErr);

                Task.WaitAll(cmd.ExecuteAsync());
            }

            _dockerService.StartContainer(input.ContainerId);
        }
    }
}