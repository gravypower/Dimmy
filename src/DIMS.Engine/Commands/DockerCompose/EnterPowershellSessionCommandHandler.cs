using System.Diagnostics;

namespace DIMS.Engine.Commands.DockerCompose
{
    public class EnterPowershellSessionCommandHandler :ICommandHandler<EnterPowershellSession>
    {
        public void Handle(EnterPowershellSession command)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "C:\\windows\\system32\\windowspowershell\\v1.0\\powershell.exe",
                    Arguments = $"-NoLogo -NoExit -Command docker exec -it {command.ContainerId} powershell",
                    RedirectStandardOutput = false,
                    UseShellExecute = true,
                    CreateNoWindow = false,
                }
            };
            process.Start();
        }
    }
}
