namespace Lexer
{
    public class Lexer
    {
        private string _input;

        public Lexer(string input)
        {
            _input = input;
        }

        public List<Token> Tokenize()
        {
            int _position = 0;
            List<Token> tokens = new List<Token>();

            while (_position < _input.Length)
            {
                nextToken = new Token(currentChar)

                if (nextToken.TokenType == TokenType.none)
                {
                    _position++;
                    continue;
                }

                _position += nextToken.Length;
                tokens.Add(nextToken);
            }

            return tokens;
        }
    }
}