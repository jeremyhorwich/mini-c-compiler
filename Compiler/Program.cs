using Lex;
using Parser;

namespace App
{
    class Program
    {
        static void Main()
        {
            string test = "return 3;";
            var lex = new Lexer(test);
            List<Token> tokens = lex.Tokenize();
            
            Return statement = new Return(tokens);
            string code = statement.Generate();
            Console.WriteLine(code);
            
        }
    }
}
