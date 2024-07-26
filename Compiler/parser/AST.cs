using Lex;

namespace Parse
{
    public class AST
    {
        private Function? function;

        public AST(List<Token> tokens)
        {
            IEnumerator<Token> tokenEnumerator = tokens.GetEnumerator();
            function = new FunctionParser(tokenEnumerator).Parse();
        }

        public override string ToString()
        {
            if (function is not null) return function.ToString();
            return "COULD NOT GENERATE";
        }

        public string ConvertToAssembly()
        {
            if (function is null) return "COULD NOT GENERATE";  //throw error?
            string assembly = "\t.text";
            
            assembly += function.Generate();

            return assembly;
        }
    }
}