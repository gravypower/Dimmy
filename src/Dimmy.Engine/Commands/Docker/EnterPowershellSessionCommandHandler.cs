using System.Diagnostics;

namespace Dimmy.Engine.Commands.Docker
{
    public class EnterPowershellSessionCommandHandler : ICommandHandler<EnterPowershellSession>
    {
        public void Handle(EnterPowershellSession command)
        {
            var setTitleCommand = $"{{$host.ui.RawUI.WindowTitle = '{command.ShellTitle}'}}";

            var noExit = command.NoExit ? "-NoExit" : "";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "C:\\windows\\system32\\windowspowershell\\v1.0\\powershell.exe",
                    Arguments =
                        $"-NoLogo {noExit} -Command docker exec -it {command.ContainerId} powershell -NoExit -Command {setTitleCommand};",
                    RedirectStandardOutput = false,
                    UseShellExecute = true,
                    CreateNoWindow = false
                }
            };

            process.Start();
        }
    }
}