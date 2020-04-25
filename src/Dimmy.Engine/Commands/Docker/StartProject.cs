namespace Dimmy.Engine.Commands.Docker
{
    public class StartProject:ICommand
    {
        public string DockerComposeFilePath { get; set; }
    }
}
