using System;

namespace Dimmy.Engine.Pipelines.StopProject
{
    public interface IStopProjectContext
    {
        Guid ProjectId { get; set; }
    }
}