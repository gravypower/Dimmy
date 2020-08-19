using System.Collections.Generic;
using Ductus.FluentDocker.Model.Compose;

namespace Dimmy.Engine.Models.Yaml.DockerCompose
{
    public class Service
    {
        public IList<dynamic> Volumes { get; set; }
        
        public string Image { get; set; }
    }
}