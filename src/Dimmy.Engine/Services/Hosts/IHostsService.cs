using System.Collections.Generic;
using SharpHostsFile;

namespace Dimmy.Engine.Services.Hosts
{
    public interface IHostsService
    {
        void AddHostsFileEntry(IList<HostsFileEntryBase> hostsFileMapEntries);
    }
}