using System.Collections.Generic;
using System.Linq;
using SharpHostsFile;

namespace Dimmy.Engine.Services.Hosts
{
    public class HostsService:IHostsService
    {
        public void AddHostsFileEntry(IList<HostsFileEntryBase> hostsFileMapEntries)
        {
            var hostsFile = new HostsFile();
            hostsFile.Load(HostsFile.GetDefaultHostsFilePath());
            
            var addedHosts = false;
            foreach (var commandHostsFileMapEntry in hostsFileMapEntries)
            {
                if (hostsFile.Entries.Any(e => e.RawLine == commandHostsFileMapEntry.ToString())) 
                    continue;
                
                addedHosts = true;
                hostsFile.Add(commandHostsFileMapEntry);
            }

            if (addedHosts)
            {
                hostsFileMapEntries.Insert(0, new HostsFileComment("Added by DIMMY"));
                hostsFileMapEntries.Add(new HostsFileComment("End of DIMMY section "));
            }
            
            hostsFile.Save(HostsFile.GetDefaultHostsFilePath());
        }
    }
}