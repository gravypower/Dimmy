using System.Collections.Generic;
using SharpHostsFile;

namespace Dimmy.Engine.Commands.HostSystem
{
    public class AddHostsFileMapEntries : ICommand
    {
        public IList<HostsFileEntryBase> HostsFileMapEntries { get; set; }
    }
}