using Lex;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            string test = "int main() {\nreturn 2;\n}";
            var lex = new Lexer(test);
            List<Token> tokens = lex.Tokenize();
            
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
        }
    }
}
