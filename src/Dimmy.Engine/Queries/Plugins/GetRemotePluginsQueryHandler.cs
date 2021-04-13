using System.Collections.Generic;
using Dimmy.Engine.Services;
using Dimmy.Engine.Services.Nuget;
using NuGet.Protocol.Core.Types;

namespace Dimmy.Engine.Queries.Plugins
{
    public class GetRemotePluginsQueryHandler : IQueryHandler<GetRemotePlugins, IList<IPackageSearchMetadata>>
    {
        private readonly INugetService _nugetService;

        public GetRemotePluginsQueryHandler(INugetService nugetService)
        {
            _nugetService = nugetService;
        }

        public IList<IPackageSearchMetadata> Handle(GetRemotePlugins query)
        {
            return _nugetService.GetNugetPackagesFromTag("dimmyplugin").Result;
        }
    }
}