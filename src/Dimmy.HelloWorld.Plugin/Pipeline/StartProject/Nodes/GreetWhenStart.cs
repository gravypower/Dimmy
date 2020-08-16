using System;
using Dimmy.Engine.Pipelines;
using Dimmy.Engine.Pipelines.StartProject;

namespace Dimmy.HelloWorld.Plugin.Pipeline.StartProject.Nodes
{
    public class GreetWhenStart: Node<IStartProjectContext>
    {
        public override void DoExecute(IStartProjectContext input)
        {
            Console.WriteLine("Hello from the Hello World plugin. This is a step in the Project Start Pipeline");
        }
    }
}