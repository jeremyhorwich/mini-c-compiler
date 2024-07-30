namespace Lex
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
            int line = 1;
            List<Token> tokens = new List<Token>();

            while (_position < _input.Length)
            {
                var nextToken = new Token(_input.Substring(_position));
                if (nextToken.Type == TokenType.newLine)
                {
                    _position++;
                    line++;
                    continue;
                }
                try 
                {
                    if (nextToken.Type == TokenType.none)
                    {
                        string message = $"Could not scan token {nextToken.Value} at line {line}";
                        throw new Exception(message);
                    }
                }
                catch
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