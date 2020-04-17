using System;
using System.IO;
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
        /// Support plugins are available for:
        ///   - JetBrains ReSharper        https://nuke.build/resharper
        ///   - JetBrains Rider            https://nuke.build/rider
        ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
        ///   - Microsoft VSCode           https://nuke.build/vscode

        public static int Main () => Execute<Build>(x => x.Compile);

        [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
        readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
        
        [Solution] readonly Solution Solution;
        [GitRepository] readonly GitRepository GitRepository;
        [GitVersion] readonly GitVersion GitVersion;

        AbsolutePath SourceDirectory => RootDirectory / "src";
        AbsolutePath TestsDirectory => RootDirectory / "tests";
        AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

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
                    .SetProjectFile(Solution)
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
                    .SetProject(RootDirectory / "src" / "Dimmy.Cli.Application")
                    .EnableIncludeSymbols()
                    .SetVersionSuffix(GitVersion.Sha));
            });

        Target InstallPlugins => _ => _
            .DependsOn(Restore)
            .Executes(() =>
            {
                var t = new DirectoryInfo(SourceDirectory);

                foreach (var absolutePath in t.GetDirectories("*plugin"))
                {
                    
                }
            });
    }
}