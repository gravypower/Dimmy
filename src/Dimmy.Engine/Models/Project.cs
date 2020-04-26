using System;
using System.Collections.Generic;

namespace Dimmy.Engine.Models
{
    public class Project
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public IList<Service> Services { get; set; } = new List<Service>();
    }
}
