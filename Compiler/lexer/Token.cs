namespace Lexer 
{
    public enum TokenType
    {
        intKeyword,
        main,
        openParantheses,
        closedParentheses,
        openBrace,
        closeBrace,
        returnKeyword,
        constant,
        semicolon,
    }

    public class Token
    {
        public TokenType Type;
        public string Value;

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
