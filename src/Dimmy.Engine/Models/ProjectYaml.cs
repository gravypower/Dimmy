using System;
using System.Collections.Generic;

namespace Dimmy.Engine.Models
{
    public class ProjectYaml
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string ComposeTemplate { get; set; }
        public IDictionary<string, string> VariableDictionary { get; set; } = new Dictionary<string, string>();
    }
}
