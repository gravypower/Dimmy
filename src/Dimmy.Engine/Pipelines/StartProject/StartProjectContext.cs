namespace Dimmy.Engine.Pipelines.StartProject
{
    public class StartProjectContext:IStartProjectContext
    {
        public Commands.Docker.StartProject Command { get; set; }
    }
}