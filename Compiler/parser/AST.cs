using Lex;
using CompilerUtility;

namespace Parse
{
    public class AST
    {
        private Function? function;

        public AST(List<Token> tokens)
        {
            SmartList<Token> smartTokenList = new SmartList<Token>(tokens);
            function = new FunctionParser(smartTokenList).Parse();
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