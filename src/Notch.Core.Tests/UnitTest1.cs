using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Notch.Core.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void FindApisInManuallyBuiltHierarchy()
        {
            var root = CreateHierarchyManually();
            var apis = 
                root.Find(n => n.Content.Trim().ToUpper() == "API");
            Assert.Equal(2,apis.Count());
        }


        [Fact]
        public void FindApisInParsedHierarchy()
        {
            var root = ParseHierarchyHappyPath();
            var apis =
                root.Find(n => n.Content.Trim().ToUpper() == "API");

            Assert.Equal(2, apis.Count());

        }

        [Fact]
        public void GenerateApiEndpointClassesFromParsedHierarchy()
        {
            var root = ParseHierarchyHappyPath();
            var apis =
                root.Find(n => n.Content.Trim().ToUpper() == "API").ToList();

            
            var code = "";

            foreach (var api in apis)
            {
                code += CreateApiEndpointClasses(api);
            }

            _output.WriteLine(code);

            Assert.Equal(2, apis.Count);

        }

        private string CreateApiEndpointClasses(Node api)
        {
            var endpoint = api.Children[0];
            var result = "";

            var methods = endpoint.Children[0].Children;

            foreach (var method in methods)
            {
                result+=$"public class {method.Content}Endpoint : ApiEndpoint<{endpoint.Content}>\r\n" + "{";

                result += "\r\n\tpublic IActionResult Execute()\r\n";
                result += "\t{\r\n\t\t /* future method */ \r\n\t}\r\n";
                result += "}\r\n\r\n";
            }

            return result;
        }


        private Node CreateHierarchyManually()
        {
            var rootNode = new Node();

            var server = rootNode.Add(new Node { Content = "Server" });

            var api = server.Add(new Node { Content = "API" });

            var methods = api.Add(new Node { Content = "Method" });

            methods.Add(new Node { Content = "Parse" });

            methods.Add(new Node { Content = "Compile" });

            api = server.Add(new Node { Content = "API" });

            methods = api.Add(new Node { Content = "Method" });

            methods.Add(new Node { Content = "Google" });

            methods.Add(new Node { Content = "Bing" });

            return rootNode;
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
    }
}
