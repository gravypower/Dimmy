﻿namespace Dimmy.Engine.Pipelines.StartProject
{
    public interface IStartProjectContext
    {
        public string WorkingPath { get; set; }
        public bool GeneratOnly { get; set; }
    }
}