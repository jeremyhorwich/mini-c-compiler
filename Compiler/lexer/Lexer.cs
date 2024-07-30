namespace Lex
{
    public class Lexer
    {
        private string _input;
        public bool hadError { get; private set; } = false;

        public Lexer(string input)
        {
            _input = input;
        }

        public List<Token> Tokenize()
        {
            int position = 0;
            int column = 1;
            int line = 1;
            List<Token> tokens = new List<Token>();

            while (position < _input.Length)
            {
                var nextToken = new Token(_input.Substring(position));
                if (nextToken.Type == TokenType.newLine)
                {
                    position++;
                    line++;
                    column = 0;
                    tokens.Add(nextToken);
                    continue;
                }
                if (nextToken.Type == TokenType.whiteSpace)
                {
                    position++;
                    column++;
                    continue;
                }
                try 
                {
                    if (nextToken.Type == TokenType.none)
                    {
                        hadError = true;
                        string message = $"Could not scan token {nextToken.Value} at line {line}, column {column}";
                        throw new Exception(message);
                    }
                }
                catch
                {
                    position++;
                    column++;
                    continue;
                }
                position += nextToken.Length;
                column += nextToken.Length;
                tokens.Add(nextToken);
            }
            tokens.Add(new Token(TokenType.endOfFile));

            return tokens;
        }
    }
}