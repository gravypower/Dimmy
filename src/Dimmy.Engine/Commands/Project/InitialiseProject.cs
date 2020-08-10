﻿using System.Collections.Generic;

namespace Dimmy.Engine.Commands.Project
{
    public class InitialiseProject : ICommand
    {
        public string Name { get; set; }
        public string SourceCodePath { get; set; }
        public string WorkingPath { get; set; }
        public string DockerComposeTemplatePath { get; set; }
        public IDictionary<string, string> PublicVariables { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> PrivateVariables { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> MetaData { get; set; } = new Dictionary<string, string>();
    }
}