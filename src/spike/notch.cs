
void Main()
{
//	var root = CreateHierarchyManually();
var root = ParseHierarchyHappyPath();
//	root.Dump();

root.Find(n=> n.Content.Trim().ToUpper()=="API").Dump();
}



public Node ParseHierarchyHappyPath()
{
	var crlf = "\r\n";
	var tab = "\t";

	var text = 
		"Database Server" + crlf +
					tab + "Database" + crlf +
					tab + tab + "Tables" + crlf +
					tab + tab + tab + "Customer" + crlf +
					tab + tab + tab + tab + "Fields" + crlf +
					tab + tab + tab + tab + tab + "FirstName" + crlf +
					tab + tab + tab + tab + tab + "LastName" + crlf +
					tab + tab + tab + "Address" + crlf +
					tab + tab + tab + tab + "Fields" + crlf +
					tab + tab + tab + tab + tab + "AddressLine" + crlf +
					tab + tab + tab + tab + tab + "City" + crlf +
					tab + tab + tab + tab + tab + "State" + crlf +
					tab + tab + tab + tab + tab + "PostalCode" + crlf +
					tab + tab + tab + tab + tab + "CountryCode" + crlf +
		"Server" + crlf +
					tab + "Api" + crlf +
					tab + tab + "Customer" + crlf +
					tab + tab + tab + "Method" + crlf +
					tab + tab + tab + tab + "AddCustomer" + crlf +
					tab + tab + tab + tab + "DeleteCustomer" + crlf +
					tab + "Api" + crlf +
					tab + tab + "Address" + crlf +
					tab + tab + tab + "Method" + crlf +
					tab + tab + tab + tab + "AddAddress" + crlf +
					tab + tab + tab + tab + "DeleteAddress" + crlf;

	var parser = new NodeParser();

	return parser.Parse(text);
}

public Node CreateHierarchyManually()
{
	var rootNode = new Node();

	var server = rootNode.Add(new Node { Content = "Server" });

	var api = server.Add(new Node { Content = "API" });

	var methods = api.Add(new Node { Content = "Method" });

	var method1 = methods.Add(new Node { Content = "Parse" });

	var method2 = methods.Add(new Node { Content = "Compile" });

	api = server.Add(new Node { Content = "API" });

	methods = api.Add(new Node { Content = "Method" });

	method1 = methods.Add(new Node { Content = "Google" });

	method2 = methods.Add(new Node { Content = "Bing" });

	return rootNode;
}

public class NodeParser
{
	public Node Parse(string content)
	{
		var root = new Node();
		var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
		var levels = new Dictionary<int, Node>();
		levels[0] = root;

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
}

