using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Notch.Core
{
    public class Node
    {
        public Node Parent { get; set; }
        public string Content { get; set; }
        public List<Node> Children { get; set; } = new List<Node>();

        public IEnumerable<Node> Find(Func<Node, IEnumerable<Node>> f)
        {
            return this.Children.SelectMany(n => f(n));
        }

        public IEnumerable<Node> Find(Func<Node, bool> f)
        {
            return this.Children.SelectMany(n => n.Children.Where(nn => f(nn)));
        }

        public Node Add(Node child)
        {
            child.Parent = this;
            Children.Add(child);
            return child;
        }

        public override string ToString()
        {
            
            return Content;
        }
    }
}
