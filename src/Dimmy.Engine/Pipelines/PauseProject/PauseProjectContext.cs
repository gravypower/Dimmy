using System;

namespace Dimmy.Engine.Pipelines.PauseProject
{
    public class PauseProjectContext : IPauseProjectContext
    {
        public Guid ProjectId { get; set; }
    }
}