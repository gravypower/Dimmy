﻿using System.IO;
using System.Threading.Tasks;

namespace Dimmy.Engine.Commands.Project
{
    public class SaveProjectYamlCommandHandler:ICommandHandler<SaveProjectYaml>
    {
        public Task Handle(SaveProjectYaml command)
        {
            return Task.Run(() => Run(command));
        }

        private static void Run(SaveProjectYaml command)
        {
            var serializer = new YamlDotNet.Serialization.Serializer();
            var yaml = serializer.Serialize(command.ProjectYaml);

            File.WriteAllText($"{command.SavePath}\\.dimmy.yaml", yaml);
        }
    }
}