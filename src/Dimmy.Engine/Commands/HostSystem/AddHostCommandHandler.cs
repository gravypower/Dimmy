using System.Linq;
using SharpHostsFile;

namespace Dimmy.Engine.Commands.HostSystem
{
    public class AddHostsFileMapEntriesCommandHandler : ICommandHandler<AddHostsFileMapEntries>
    {
        public void Handle(AddHostsFileMapEntries command)
        {
            var hostsFile = new HostsFile();
            hostsFile.Load(HostsFile.GetDefaultHostsFilePath());
            
            command.HostsFileMapEntries.Insert(0, new HostsFileComment("Added by DIMMY"));
            command.HostsFileMapEntries.Add(new HostsFileComment("End of DIMMY section "));
            
            foreach (var commandHostsFileMapEntry in command.HostsFileMapEntries)
            {
                if (hostsFile.Entries.All(e => e.RawLine != commandHostsFileMapEntry.ToString()))
                {
                    hostsFile.Add(commandHostsFileMapEntry);
                }
            }
            
            hostsFile.Save(HostsFile.GetDefaultHostsFilePath());
        }
    }
}