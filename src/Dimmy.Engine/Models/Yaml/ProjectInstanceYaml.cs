using System;
using System.Collections.Generic;

namespace Dimmy.Engine.Models.Yaml
{
    public class ProjectInstanceYaml
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string SourceCodeLocation { get; set; }
        public string WorkingPath { get; set; }
        public IDictionary<string, string> VariableDictionary { get; set; } = new Dictionary<string, string>();
    }
}