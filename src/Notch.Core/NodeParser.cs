using System;
using System.Collections.Generic;
using System.Linq;

namespace Notch.Core
{
	public class NodeParser
    {
        public Node Parse(string content)
        {
            var root = new Node();
            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var levels = new Dictionary<int, Node>();
            levels[-1] = root;

            foreach (var line in lines)
            {
                var indent = 0;
                var startingTextPosition = 0;

                foreach (var ch in line)
                {
                    if (ch == ' ')
                    {
                        startingTextPosition++;
                        continue;
                    }

                    if (ch == '\t')
                    {
                        startingTextPosition++;
                        indent++;
                        continue;
                    }

                    break;
                }

                var item = line.Substring(startingTextPosition);
                var node = new Node();
                node.Content = item;

                if (levels.Keys.Contains(indent - 1))
                {
                    levels[indent - 1].Add(node);
                    levels[indent] = node;
                }
                else
                {
                    levels[indent].Add(node);
                    levels[indent] = node;
                }
            }
            return root;
        }
    }
}
