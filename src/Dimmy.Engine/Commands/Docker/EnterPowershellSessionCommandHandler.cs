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
                    FileName = "powershell",
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