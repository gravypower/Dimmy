namespace Dimmy.VisualStudio.Plugin.Services.VisualStudioRemoteTools
{

    //https://chocolatey.org/packages/visualstudio2019-remotetool
    public class VisualStudio2019x86:IVisualStudioRemoteTool
    {
        public string VisualStudioVersion => "2019";
        public string Architecture => "x86";
        public string ToolFileName => "VS_RemoteTools.exe";
        public string Url => "https://download.visualstudio.microsoft.com/download/pr/208cc0c2-6455-4c15-86b2-33a54ed54739/bac1d64ecf94f0f61eb75fcf27fcaa60600d3f2f4f99558d8c0e01da651545e2/VS_RemoteTools.exe";
        public string Checksum => "BAC1D64ECF94F0F61EB75FCF27FCAA60600D3F2F4F99558D8C0E01DA651545E2";
        public string ChecksumType => "sha256";
    }
}
