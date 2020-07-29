using System;
using Dimmy.Engine.Models;

namespace Dimmy.Cli.Extensions
{
    public static class ProjectPrettyPrint
    {
        public static void PrettyPrint(this Project p)
        {
            Console.WriteLine($"\\-{p.Name}");

            foreach (var role in p.Services) Console.WriteLine($"  |-{role.Name}");
        }
    }
}