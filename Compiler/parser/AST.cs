using Lex;

namespace Parser
{
    public class AST
    {
        private Function function;

        public AST(List<Token> tokens)
        {
            function = new Function(tokens);
        }

        public override string ToString()
        {
            return function.ToString();
        }

        public string ConvertToAssembly()
        {
            string assembly = "";
            if (!function.parseValid) return "COULD NOT GENERATE";  //throw error?
            
            assembly += function.Generate();
            return assembly;
        }
    }
}