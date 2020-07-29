using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace _build
{
    [CheckBuildProjectConfigurations]
    [UnsetVisualStudioEnvironmentVariables]
    class Build : NukeBuild
    {
        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

        [GitRepository] readonly GitRepository GitRepository;
        [GitVersion] readonly GitVersion GitVersion;

        [Solution] readonly Solution Solution;

        AbsolutePath SourceDirectory => RootDirectory / "src";
        AbsolutePath TestsDirectory => RootDirectory / "tests";
        AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
        AbsolutePath CliDirectory => RootDirectory / "src" / "Dimmy.Cli.Application";


        Target Clean => _ => _
            .Before(Restore)
            .Executes(() =>
            {
                SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
                EnsureCleanDirectory(ArtifactsDirectory);
            });

        Target Restore => _ => _
            .Executes(() =>
            {
                DotNetRestore(s => s
                    .SetProjectFile(Solution));
            });

        Target Compile => _ => _
            .DependsOn(Restore)
            .Executes(() =>
            {
                DotNetBuild(s => s
                    .SetProjectFile(CliDirectory / "Dimmy.Cli.Application.csproj")
                    .SetConfiguration(Configuration)
                    .SetAssemblyVersion(GitVersion.AssemblySemVer)
                    .SetFileVersion(GitVersion.AssemblySemFileVer)
                    .SetInformationalVersion(GitVersion.InformationalVersion)
                    .EnableNoRestore());
            });

        Target Pack => _ => _
            .DependsOn(Compile)
            .Executes(() =>
            {
                EnsureExistingDirectory(ArtifactsDirectory);

                DotNetPack(settings => settings
                    .SetOutputDirectory(ArtifactsDirectory)
                    .SetProject(CliDirectory)
                    .EnableIncludeSymbols()
                    .SetVersionSuffix(GitVersion.Sha));
            });

        Target PublishPlugins => _ => _
            .Executes(() =>
            {
                var di = new DirectoryInfo(SourceDirectory);

                foreach (var pluginDirectory in di.GetDirectories("*plugin"))
                {
                    var projectFiles = pluginDirectory.GetFiles("*.csproj");

                    DotNetPublish(s => s
                        .SetNoDependencies(true)
                        .SetProject(projectFiles.Single().FullName)
                        .SetConfiguration(Configuration)
                        .SetAssemblyVersion(GitVersion.AssemblySemVer)
                        .SetFileVersion(GitVersion.AssemblySemFileVer)
                        .SetInformationalVersion(GitVersion.InformationalVersion)
                        .EnableNoRestore()
                        .SetOutput(CliDirectory / "bin" / "Debug" / "netcoreapp3.1" / "plugins" /
                                   pluginDirectory.Name));
                }
            });

        /// Support plugins are available for:
        /// - JetBrains ReSharper        https://nuke.build/resharper
        /// - JetBrains Rider            https://nuke.build/rider
        /// - Microsoft VisualStudio     https://nuke.build/visualstudio
        /// - Microsoft VSCode           https://nuke.build/vscode
        public static int Main() => Execute<Build>(x => x.Compile);
    }
}