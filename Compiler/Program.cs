using Lex;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            string test = "{ int return }";
            var lex = new Lexer(test);
            List<Token> tokens = lex.Tokenize();
            
            foreach (Token token in tokens)
            {
                Console.WriteLine(token + "|");
            }
        }
    }
}
