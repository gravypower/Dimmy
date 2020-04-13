namespace DIMS.Engine.Commands.Docker
{
    public class EnterPowershellSession:ICommand
    {
        public string ContainerId { get; set; }
        public string ShellTitle { get; set; }
    }
}