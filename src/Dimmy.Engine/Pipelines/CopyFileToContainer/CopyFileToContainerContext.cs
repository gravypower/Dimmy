namespace Dimmy.Engine.Pipelines.CopyFileToContainer
{
    public class CopyFileToContainerContext : ICopyFileToContainerContext
    {
        public string WorkingPath { get; set; }
        public CopyFile[] CopyFiles { get; set; }
        public string ContainerId { get; set; }
    }
}
