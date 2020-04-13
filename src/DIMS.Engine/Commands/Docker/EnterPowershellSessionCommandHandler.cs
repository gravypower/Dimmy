using System.Diagnostics;
using System.Threading.Tasks;

namespace DIMS.Engine.Commands.Docker
{
    public class EnterPowershellSessionCommandHandler :ICommandHandler<EnterPowershellSession>
    {
        public Task Handle(EnterPowershellSession command)
        {
            return Task.Run(() => Run(command));
        }

        private static void Run(EnterPowershellSession command)
        {
            var setTitleCommand = $"{{$host.ui.RawUI.WindowTitle = '{command.ShellTitle}'}}";
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "C:\\windows\\system32\\windowspowershell\\v1.0\\powershell.exe",
                    Arguments =
                        $"-NoLogo -Command docker exec -it {command.ContainerId} powershell -NoExit -Command {setTitleCommand};",
                    RedirectStandardOutput = false,
                    UseShellExecute = true,
                    CreateNoWindow = false
                }
            };
            process.Start();
        }
    }
}
