using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Dimmy.Engine.Commands;
using Dimmy.VisualStudio.Plugin.Services.VisualStudioRemoteTools;

namespace Dimmy.VisualStudio.Plugin.Commands.Development
{
    public class InstallVisualStudio2019RemoteToolsCommandHandler : ICommandHandler<InstallVisualStudioRemoteTools>
    {
        private readonly IEnumerable<IVisualStudioRemoteTool> _visualStudioRemoteTools;

        public InstallVisualStudio2019RemoteToolsCommandHandler(
            IEnumerable<IVisualStudioRemoteTool> visualStudioRemoteTools)
        {
            _visualStudioRemoteTools = visualStudioRemoteTools;
        }


        public Task Handle(InstallVisualStudioRemoteTools command)
        {
            return Task.Run(() => Run(command));
        }

        private void Run(InstallVisualStudioRemoteTools command)
        {
            var remoteTool = _visualStudioRemoteTools
                .Single(t =>
                    t.VisualStudioVersion == command.VisualStudioVersion && t.Architecture == command.Architecture);

            using (var sha256 = SHA256.Create())
            {
                using (var remoteToolStream = new MemoryStream(new WebClient().DownloadData(remoteTool.Url)))
                {
                    var checkSum = sha256.ComputeHash(remoteToolStream);
                    var checkSumString = BitConverter.ToString(checkSum).Replace("-", string.Empty);

                    if (remoteTool.Checksum != checkSumString) throw new CheckSumVerificationFailed();

                    var exists = Directory.Exists(command.InstallPath);

                    if (!exists)
                        Directory.CreateDirectory(command.InstallPath);

                    var remoteToolPath = Path.Combine(command.InstallPath, remoteTool.ToolFileName);
                    using (var file = new FileStream(remoteToolPath, FileMode.Create, FileAccess.Write))
                    {
                        var bytes = new byte[remoteToolStream.Length];
                        remoteToolStream.Read(bytes, 0, (int) remoteToolStream.Length);
                        file.Write(bytes, 0, bytes.Length);
                        remoteToolStream.Close();
                    }
                }
            }
        }
    }

    internal class CheckSumVerificationFailed : Exception
    {
    }
}