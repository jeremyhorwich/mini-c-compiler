using Lex;

namespace Parser
{
    public class AST
    {
        List<Node> nodes = new List<Node>();

        public AST(List<Token> tokens)
        {
            //Get our list of nodes
        }

        public override string ToString()
        {
            //Pretty print AST
            return "";
        }

        public string ConvertToAssembly()
        {
            string assembly = "";
            foreach (Node node in nodes)
            {
                assembly += node.Generate();
            }

            return assembly;
        }
    }
}