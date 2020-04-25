using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dimmy.Engine.NuGet;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace Dimmy.Engine.Queries.Plugins
{
    public class GetRemotePluginsQueryHandler:IQueryHandler<GetRemotePlugins, IAsyncEnumerable<string>>
    {
        public Task<IAsyncEnumerable<string>> Handle(GetRemotePlugins query)
        {

            return Task.Run(() => Run(query));
        }

        private async IAsyncEnumerable<string> Run(GetRemotePlugins query)
        { 
            var settings = Settings.LoadDefaultSettings(root: null);
            var packageSourceProvider = new PackageSourceProvider(settings);
            var sourceRepositoryProvider =
                new SourceRepositoryProvider(packageSourceProvider, Repository.Provider.GetCoreV3());

            using var cacheContext = new SourceCacheContext();
            var repositories = sourceRepositoryProvider.GetRepositories();

            foreach (var sourceRepository in repositories)
            {
                var searchResource = await sourceRepository.GetResourceAsync<PackageSearchResource>();
                var searchMetadata = await searchResource.SearchAsync(
                    @"Tags:""Dimmy-plugin""",
                    new SearchFilter(true),
                    0,
                    10,
                    new TextWriterLogger(Console.Out),
                    CancellationToken.None);

                foreach (var packageSearchMetadata in searchMetadata)
                {
                    yield return packageSearchMetadata.Identity.Id;
                }
            }

        }
    }
}
