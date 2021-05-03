namespace Dimmy.Engine.Pipelines.CopyFileToContainer
{
    public interface ICopyFileToContainerContext
    {
        string WorkingPath { get; set; }
        CopyFile[] CopyFiles { get; set; }
        public string ContainerId { get; set; }
    }
}