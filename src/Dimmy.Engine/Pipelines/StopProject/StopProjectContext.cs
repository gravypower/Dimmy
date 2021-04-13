using System;

namespace Dimmy.Engine.Pipelines.StopProject
{
    public class StopProjectContext : IStopProjectContext
    {
        public Guid ProjectId { get; set; }
    }
}