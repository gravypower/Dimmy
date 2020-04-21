namespace Dimmy.VisualStudio.Plugin.Services.VisualStudioRemoteTools
{
    //https://chocolatey.org/packages/visualstudio2019-remotetool
    public class VisualStudio2019x64:IVisualStudioRemoteTool
    {
        public string VisualStudioVersion => "2019";
        public string Architecture => "x64";
        public string ToolFileName => "VS_RemoteTools.exe";
        public string Url => "https://download.visualstudio.microsoft.com/download/pr/208cc0c2-6455-4c15-86b2-33a54ed54739/7ec6144f19125bcf1c2c32f5cd8758846172f38528dd93a889de4177f2ef3d27/VS_RemoteTools.exe";
        public string Checksum => "7EC6144F19125BCF1C2C32F5CD8758846172F38528DD93A889DE4177F2EF3D27";
        public string ChecksumType => "sha256";
    }
}
