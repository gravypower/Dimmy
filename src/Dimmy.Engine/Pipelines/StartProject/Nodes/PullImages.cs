using System;
using System.Threading.Tasks;
using Ductus.FluentDocker.Commands;
using Ductus.FluentDocker.Services;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class PullImages:Node<IStartProjectContext>
    {
        public override int Order => -3;

        public PullImages()
        {
        }
        public override async Task DoExecute(IStartProjectContext input)
        {
            return;
            
            // var p = new DataReceived();
            //
            // p.ErrorDataReceived += (sender, s) =>    
            // {
            //     if(s.ProcessIdentifier != nameof(Client.Pull))
            //         return;
            //     
            //     if (!string.IsNullOrEmpty(s.Data))
            //         Console.Error.Write(s.Data);
            // };
            //
            // p.OutputDataReceived += (sender, s) => 
            // {
            //     if(s.ProcessIdentifier != nameof(Client.Pull))
            //         return;
            //     
            //     if (!string.IsNullOrEmpty(s.Data))
            //         Console.WriteLine(s.Data);
            // };
            //
            // using (DataReceivedContext.UseProcessManager(p))
            // {
            //     foreach (var v in input.Project.VariableDictionary)
            //     {
            //         if (v.Key.EndsWith("Image"))
            //         {
            //             _host.Host.Pull(v.Value);
            //         }
            //     }
            // }
        }
    }
}