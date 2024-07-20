using Lex;
using Parser;

namespace App
{
    class Program
    {
        static void Main()
        {
            string test = "int foo() {return 450;}";
            var lex = new Lexer(test);
            List<Token> tokens = lex.Tokenize();
            
            Function statement = new Function(tokens);
            string code = statement.Generate();
            Console.WriteLine(code);         
        }
    }
}
