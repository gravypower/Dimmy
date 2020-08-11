using System.Net;
using SharpHostsFile;

namespace Dimmy.Engine.Commands.HostSystem
{
    public class AddHostCommandHandler : ICommandHandler<AddHost>
    {
        public void Handle(AddHost command)
        {
            var hostsFile = new HostsFile();
            hostsFile.Load(HostsFile.GetDefaultHostsFilePath());
            hostsFile.Add(new HostsFileMapEntry(command.Address, command.Hostname, command.Comment));
            hostsFile.Save(HostsFile.GetDefaultHostsFilePath());
        }
    }
}