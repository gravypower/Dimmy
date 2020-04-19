using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Dimmy.Cli.Application;
using NuGet.Packaging;

namespace Dimmy.Cli.Plugins
{

    class Loader
    {
        private static readonly string PluginDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        private static readonly DirectoryInfo PluginDirectory = new DirectoryInfo(PluginDirectoryPath);
        public void Load()
        {
            var logger = new TextWriterLogger(Console.Out);
            //var repository = Repository.Factory.GetCoreV3(@"https://api.nuget.org/v3/index.json");

            foreach (var nupkgFile in PluginDirectory.GetFiles())
            {
                var path = Path.Combine(PluginDirectoryPath, Path.GetFileNameWithoutExtension(nupkgFile.Name));
                if (Directory.Exists(path)) continue;

                var nupkgFileStream = File.Open(nupkgFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                var packageArchiveReader = new PackageArchiveReader(nupkgFileStream);

                Directory.CreateDirectory(path);

                foreach (var frameworkSpecificGroup in packageArchiveReader.GetLibItems())
                {
                    foreach (var item in frameworkSpecificGroup.Items)
                    {
                        var libPath = Path.Combine(path, Path.GetFileName(item));
                        packageArchiveReader.ExtractFile(item, libPath, logger);
                    }
                }

                //var finder = repository.GetResource<FindPackageByIdResource>();

                //var dependencies = packageArchiveReader.GetPackageDependencies()
                //    .SelectMany(item => item.Packages)
                //    .ToList();

                //var sourceCacheContext = new SourceCacheContext();

                //foreach (var dependency in dependencies)
                //{
                //    var versions = finder.GetAllVersionsAsync(dependency.Id, sourceCacheContext, logger, CancellationToken.None).Result;

                //    var version = dependency.VersionRange.FindBestMatch(versions);
                //    var ms = new MemoryStream();
                //    if (!finder.CopyNupkgToStreamAsync(dependency.Id, version, ms, sourceCacheContext, logger, CancellationToken.None).Result) continue;

                //    var d = new PackageArchiveReader(ms);
                //    foreach (var dd in d.GetLibItems())
                //    {
                //        foreach (var ddItem in dd.Items)
                //        {
                //            d.ExtractFile(ddItem, packageFolderName, null);
                //        }
                //    }
                //}
            }



            foreach (var directoryInfo in PluginDirectory.GetDirectories())
            {
                var pluginAssemblies =
                    from pluginFile in directoryInfo.GetFiles()
                    where pluginFile.Extension.ToLower() == ".dll"
                    select Assembly.Load(AssemblyName.GetAssemblyName(pluginFile.FullName));


            }
        }
    }
}
