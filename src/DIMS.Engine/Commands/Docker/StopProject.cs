using System;

namespace DIMS.Engine.Commands.Docker
{
    public class StopProject:ICommand
    {
        public Guid ProjectId { get; set; }
    }
}
