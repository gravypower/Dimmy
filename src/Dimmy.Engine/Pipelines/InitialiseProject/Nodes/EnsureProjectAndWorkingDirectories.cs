using System.IO;
using System.Threading.Tasks;

namespace Dimmy.Engine.Pipelines.InitialiseProject.Nodes
{
    public class EnsureProjectAndWorkingDirectories: Node<IInitialiseProjectContext>
    {
        public override int Order => -1;
        public override void DoExecute(IInitialiseProjectContext input)
        {
            if (!Directory.Exists(input.WorkingPath))
                Directory.CreateDirectory(input.WorkingPath);
            
            if (!Directory.Exists(input.SourceCodePath))
                Directory.CreateDirectory(input.SourceCodePath);
        }
    }
}