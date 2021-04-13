using System.Collections.Generic;
using SharpHostsFile;

namespace Dimmy.Engine.Services.Hosts
{
    public interface IHostsFileService
    {
        void AddHostsFileEntry(IList<HostsFileEntryBase> hostsFileMapEntries);
    }
}