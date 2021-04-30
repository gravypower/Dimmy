namespace Dimmy.Engine.Pipelines.CopyFileToContainer
{
    public interface ICopyFileToContainerContext
    {
        string WorkingPath { get; set; }
        public string TargetFilePath { get; set; }
        public string DestinationFilePath { get; set; }
        
        public string ContainerId { get; set; }
    }
}