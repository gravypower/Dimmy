using System.IO;
using YamlDotNet.Serialization;

namespace Dimmy.Engine.Commands.Project
{
    public class SaveProjectYamlCommandHandler : ICommandHandler<SaveProjectYaml>
    {
        public void Handle(SaveProjectYaml command)
        {
            var serializer = new Serializer();
            var yaml = serializer.Serialize(command.ProjectYaml);

            File.WriteAllText($"{command.SavePath}\\.dimmy.yaml", yaml);
        }
    }
}