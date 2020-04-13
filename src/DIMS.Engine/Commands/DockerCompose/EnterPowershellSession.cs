namespace DIMS.Engine.Commands.DockerCompose
{
    public class EnterPowershellSession:ICommand
    {
        public string ContainerId { get; set; }
        public string ShellTitle { get; set; }
    }
}