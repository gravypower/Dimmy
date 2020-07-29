using System;

namespace Dimmy.Engine.Commands.Docker
{
    public class StopProject : ICommand
    {
        public Guid ProjectId { get; set; }
    }
}