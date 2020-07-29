namespace Dimmy.VisualStudio.Plugin.Services.VisualStudioRemoteTools
{
    public interface IVisualStudioRemoteTool
    {
        string VisualStudioVersion { get; }
        string Architecture { get; }

        string ToolFileName { get; }
        string Url { get; }
        string Checksum { get; }
        string ChecksumType { get; }
    }
}