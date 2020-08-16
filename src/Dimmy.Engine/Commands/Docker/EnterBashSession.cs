namespace Dimmy.Engine.Commands.Docker
{
    public class EnterBashSession : ICommand
    {
        public string ContainerId { get; set; }
        public string ShellTitle { get; set; }
        public bool NoExit { get; set; }
    }
}