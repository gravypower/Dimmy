using System;

namespace Dimmy.Engine.Commands.Docker
{
    public class PauseProject : ICommand
    {
        public Guid ProjectId { get; set; }
    }
}