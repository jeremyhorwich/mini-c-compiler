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
            if (!function.parseValid) return "COULD NOT GENERATE";  //throw error?
            string assembly = ".global _start";
            
            assembly += function.Generate();
            assembly += "\n";
            assembly += $"""
            
            _start:
                call    _{function.identifier}
                mov     $60, %rax
                xor     %rdi, %rdi
                syscall
            """;

            return assembly;
        }
    }
}