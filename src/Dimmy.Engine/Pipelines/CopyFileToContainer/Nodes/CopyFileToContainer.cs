using System;
using System.IO;
using System.Threading.Tasks;
using CliWrap;
using Dimmy.Engine.Services.Docker;
using Dimmy.Engine.Services.Projects;
using Ductus.FluentDocker.Services;

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
        public override async Task DoExecute(ICopyFileToContainerContext input)
        {
            await _dockerService.StopContainer(input.ContainerId);
            
            await using var stdOut = Console.OpenStandardOutput();
            await using var stdErr = Console.OpenStandardError();
            var fileName = Path.GetFileName(input.TargetFilePath);
            var cmd = Cli.Wrap("docker")
                          .WithArguments(new[] {
                              "cp",
                              input.TargetFilePath,
                              $"{input.ContainerId}:{input.DestinationFilePath}\\{fileName}"
                          })
                          .WithWorkingDirectory(input.WorkingPath)
                      | (stdOut, stdErr);
            
            await cmd.ExecuteAsync();
            
            _dockerService.StartContainer(input.ContainerId);
        }
    }
}