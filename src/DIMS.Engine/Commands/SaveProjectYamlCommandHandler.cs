using System.IO;

namespace DIMS.Engine.Commands
{
    public class SaveProjectYamlCommandHandler:ICommandHandler<SaveProjectYaml>
    {
        public void Handle(SaveProjectYaml command)
        {
            var serializer = new YamlDotNet.Serialization.Serializer();
            var yaml = serializer.Serialize(command.ProjectYaml);

            File.WriteAllText($"{command.SavePath}\\dimsProject.yaml", yaml);
        }
    }
}
