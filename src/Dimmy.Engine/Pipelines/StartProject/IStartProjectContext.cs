﻿using Dimmy.Engine.Models.Yaml;

 namespace Dimmy.Engine.Pipelines.StartProject
{
    public interface IStartProjectContext
    {
        public ProjectInstanceYaml ProjectInstance { get; set; } 
        public ProjectYaml Project { get; set; }
        public string WorkingPath { get; set; }
        public bool GeneratOnly { get; set; }
    }
}