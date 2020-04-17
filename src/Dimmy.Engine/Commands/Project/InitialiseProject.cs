using Dimmy.Engine.Models;

namespace Dimmy.Engine.Commands.Project
{
    public class InitialiseProject : ICommand
    {
        public string SourceCodeLocation { get; set; }
        public string ProjectLocation { get; set; }
        public DockerComposeTemplate DockerComposeTemplate { get; set; }
    }
}
