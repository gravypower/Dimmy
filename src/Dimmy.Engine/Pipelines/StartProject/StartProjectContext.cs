﻿using Dimmy.Engine.Models.Yaml;
using Ductus.FluentDocker.Model.Compose;

namespace Dimmy.Engine.Pipelines.StartProject
{
    public class StartProjectContext : IStartProjectContext
    {
        public DockerComposeFileConfig DockerComposeFileConfig { get; set; }
        public ProjectInstanceYaml ProjectInstance { get; set; } 
        public ProjectYaml Project { get; set; }
        public string WorkingPath { get; set; }
        public bool GenerateOnly { get; set; }
    }
}