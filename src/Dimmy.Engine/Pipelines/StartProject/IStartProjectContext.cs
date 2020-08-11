﻿namespace Dimmy.Engine.Pipelines.StartProject
{
    public interface IStartProjectContext
    {
        public Commands.Docker.StartProject Command { get; set; }
    }
}