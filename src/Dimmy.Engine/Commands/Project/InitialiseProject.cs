using System.Collections.Generic;
using Dimmy.Engine.Models;

namespace Dimmy.Engine.Commands.Project
{
    public class InitialiseProject : ICommand
    {
        public string Name { get; set; }
        public string SourceCodePath { get; set; }
        public string WorkingPath { get; set; }
        public DockerComposeTemplate DockerComposeTemplate { get; set; }

        public IDictionary<string, string> PublicVariables { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> PrivateVariables { get; set; } = new Dictionary<string, string>();

    }
}
