using System;
using System.Collections.Generic;

namespace Dimmy.Engine.Models.Yaml
{
    public class ProjectYaml
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string ComposeTemplateFileName { get; set; }
        public IDictionary<string, string> VariableDictionary { get; set; } = new Dictionary<string, string>();
    }
}
