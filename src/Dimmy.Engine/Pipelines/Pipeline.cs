using System.Collections.Generic;
using System.Linq;

namespace Dimmy.Engine.Pipelines
{
    public abstract class Pipeline<TNode, TContext>
        where  TNode : Node<TContext>
    {
        protected TContext Context { get; set; }

        private readonly List<TNode> _nodes;

        protected Pipeline(IEnumerable<TNode> nodes)
        {
            _nodes = nodes
                .OrderBy(n=>n.Order)
                .ToList();
        }

        public void Execute(TContext context = default)
        {
            var root = default(TNode);
            var previous = default(TNode);

            if (context != null)
            {
                Context = context;
            }

            foreach (var node in _nodes)
            {
                if (root == null)
                {
                    root = node;
                }
                else
                {
                    previous.Register(node);
                }
                previous = node;
            }

            root?.Execute(context);
        }        
    }
}