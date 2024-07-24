using Lex;

namespace Parse
{
    public class AST
    {
        private Function function;

        public AST(List<Token> tokens)
        {
            IEnumerator<Token> tokenEnumerator = tokens.GetEnumerator();
            Function program = Parsers.ParseFunction(tokenEnumerator);
            function = new Function(tokens);
        }

        public override string ToString()
        {
            return function.ToString();
        }

        public string ConvertToAssembly()
        {
            if (!function.parseValid) return "COULD NOT GENERATE";  //throw error?
            string assembly = "\t.text";
            
            assembly += function.Generate();

            return assembly;
        }
    }
}