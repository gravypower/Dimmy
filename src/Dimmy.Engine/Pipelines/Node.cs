using System.Threading.Tasks;

namespace Dimmy.Engine.Pipelines
{
    public abstract class Node<TContext>
    {
        private Node<TContext> _nextNode;
        protected TContext Context { get; set; }

        public virtual int Order { get; } = 0;
        public abstract Task DoExecute(TContext input);

        public void Execute(TContext context)
        {
            Context = context;
            DoExecute(context).Wait();

            if (_nextNode != null)
            {
                _nextNode.Execute( context);
            }
        }

        public void Register(Node<TContext> nextNode)
        {
            _nextNode = nextNode;
        }
    }
}