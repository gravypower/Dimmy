using System.Collections.Generic;
using System.Threading.Tasks;
using Dimmy.Engine.Services;
using NuGet.Protocol.Core.Types;

namespace Dimmy.Engine.Queries.Plugins
{
    public class GetRemotePluginsQueryHandler:IQueryHandler<GetRemotePlugins, IAsyncEnumerable<IPackageSearchMetadata>>
    {
        private readonly INugetService _nugetService;

        public GetRemotePluginsQueryHandler(INugetService nugetService)
        {
            _nugetService = nugetService;
        }

        public Task<IAsyncEnumerable<IPackageSearchMetadata>> Handle(GetRemotePlugins query)
        {
            return Task.Run(() => Run(query));
        }

        private IAsyncEnumerable<IPackageSearchMetadata> Run(GetRemotePlugins query)
        {
            return _nugetService.GetNugetPackagesFromTag("dimmyplugin");
        }
    }
}
