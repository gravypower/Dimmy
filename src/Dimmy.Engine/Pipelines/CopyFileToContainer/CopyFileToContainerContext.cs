namespace Dimmy.Engine.Pipelines.CopyFileToContainer
{
    public class CopyFileToContainerContext : ICopyFileToContainerContext
    {
        public string WorkingPath { get; set; }
        public string TargetFilePath { get; set; }
        public string DestinationFilePath { get; set; }
        public string ContainerId { get; set; }
    }
}
