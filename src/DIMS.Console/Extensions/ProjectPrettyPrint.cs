using System;
using DIMS.Engine.Models;

namespace DIMS.CLI.Extensions
{
    public static class ProjectPrettyPrint
    {
        public static void PrettyPrint(this Project p)
        {
            Console.WriteLine($"\\-{p.Name}");

            foreach (var role in p.Roles)
            {
                Console.WriteLine($"  |-{role.Name}");
            }
        }
    }
}