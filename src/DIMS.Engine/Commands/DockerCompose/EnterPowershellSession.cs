namespace DIMS.Engine.Commands.DockerCompose
{
    public class EnterPowershellSession:ICommand
    {
        public string ContainerId { get; set; }
    }
}