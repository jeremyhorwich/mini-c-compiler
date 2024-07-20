using Lex;
using Parser;

namespace App
{
    class Program
    {
        static void Main()
        {
            string test = "int main() {return 2;}";
            var lex = new Lexer(test);
            List<Token> tokens = lex.Tokenize();
            
            Function statement = new Function(tokens);
            string code = statement.Generate();
            Console.WriteLine(code);
            Console.WriteLine(statement.ToString());         
        }
    }
}
