using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Dimmy.Cli.Commands;
using Dimmy.Cli.Commands.Project;
using Dimmy.Engine.Commands;
using Dimmy.Engine.Queries;
using Dimmy.Engine.Services;
using Ductus.FluentDocker.Services;
using NuGet.Commands;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using SimpleInjector;

namespace Dimmy.Cli.Application
{
    class Program
    {
        private static readonly Container Container;
        private static readonly string PluginDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        private static readonly DirectoryInfo PluginDirectory = new DirectoryInfo(PluginDirectoryPath);

        static Program()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

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

                NewMethod(pluginAssemblies);
            }


            Container = new Container();

            var hosts = new Hosts().Discover();
            var host = hosts.FirstOrDefault(x => x.IsNative) ?? hosts.FirstOrDefault(x => x.Name == "default"); 

            if(host == null)
                Console.WriteLine("Could not find docker!");

            Container.Register(()=>host, Lifestyle.Singleton);

            Container.Register<IProjectService, ProjectService>();

            NewMethod(assemblies);

            Container.Verify();
        }

        private static void NewMethod(IEnumerable<Assembly> a)
        {
            Container.Register(typeof(ICommandHandler<>), a);
            Container.Register(typeof(IQueryHandler<,>), a);
            Container.Collection.Register<IProjectSubCommand>(a);
            Container.Collection.Register<ICommandLineCommand>(a);
            Container.Collection.Register<InitialiseSubCommand>(a);
        }


        public static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand();

            foreach (var commandLineCommand in Container.GetAllInstances<ICommandLineCommand>())
            {
                rootCommand.AddCommand(commandLineCommand.GetCommand());
            }
            
            return await rootCommand.InvokeAsync(args);
        }
    }
}
