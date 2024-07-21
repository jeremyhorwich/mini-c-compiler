using System.Reflection.Emit;
using Lex;

namespace Parse
{
    public class Parser
    {
        private Function function;

        public Parser(List<Token> tokens)
        {
            function = new Function(tokens);
        }

        public override string ToString()
        {
            return function.ToString();
        }

        public string ConvertToAssembly()
        {
            string assembly = ".globl _start";
            if (!function.parseValid) return "COULD NOT GENERATE";  //throw error?
            
            assembly += function.Generate();
            assembly += "\n";
            assembly += $"""
            
            _start:
                call    _{function.identifier}
                mov     $1, %eax
                call    ExitProcess
            """;

            return assembly;
        }
    }
}