using Dimmy.Engine.Commands;

namespace Dimmy.VisualStudio.Plugin.Commands.Development
{
    public class InstallVisualStudioRemoteTools : ICommand
    {
        public string VisualStudioVersion { get; set; }
        public string Architecture { get; set; }
        public string InstallPath { get; set; }
    }
}