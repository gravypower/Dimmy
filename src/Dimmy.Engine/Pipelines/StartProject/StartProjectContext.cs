namespace Dimmy.Engine.Pipelines.StartProject
{
    public class StartProjectContext:IStartProjectContext
    {
        public string WorkingPath { get; set; }
        public bool GeneratOnly { get; set; }
    }
}