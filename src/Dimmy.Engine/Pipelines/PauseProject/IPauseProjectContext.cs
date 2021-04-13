using System;

namespace Dimmy.Engine.Pipelines.PauseProject
{
    public interface IPauseProjectContext
    {
        Guid ProjectId { get; set; }
    }
}