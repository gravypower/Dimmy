using System.Collections.Generic;
using NuGet.Protocol.Core.Types;

namespace Dimmy.Engine.Queries.Plugins
{
    public class GetRemotePlugins:IQuery<IAsyncEnumerable<IPackageSearchMetadata>>
    {
    }
}