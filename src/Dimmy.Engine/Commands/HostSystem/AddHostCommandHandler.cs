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
            
            var addedHosts = false;
            foreach (var commandHostsFileMapEntry in command.HostsFileMapEntries)
            {
                if (hostsFile.Entries.Any(e => e.RawLine == commandHostsFileMapEntry.ToString())) 
                    continue;
                
                addedHosts = true;
                hostsFile.Add(commandHostsFileMapEntry);
            }

            if (addedHosts)
            {
                command.HostsFileMapEntries.Insert(0, new HostsFileComment("Added by DIMMY"));
                command.HostsFileMapEntries.Add(new HostsFileComment("End of DIMMY section "));
            }
            
            hostsFile.Save(HostsFile.GetDefaultHostsFilePath());
        }
    }
}