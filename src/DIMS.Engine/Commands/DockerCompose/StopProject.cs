using System;

namespace DIMS.Engine.Commands.DockerCompose
{
    public class StopProject:ICommand
    {
        public Guid ProjectId { get; set; }
    }
}
