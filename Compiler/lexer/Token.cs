using Lexer;

namespace Lexer 
{
    public enum TokenType
    {
        keyword,
        openParantheses,
        closedParentheses,
        openBrace,
        closeBrace,
        constant,
        semicolon,
        none
    }


    private var _keywords = new List<Regex> 
    {
        //all the keywords above
    }

    public class Token
    {

        public TokenType Type (get;)
        public string Value (get;)

        public string length (get;)

        private static var _regexPatterns = new private Dictionary<TokenType, Regex>
        {
            { TokenType.OpenBrace, new Regex(@"{") },
            { TokenType.CloseBrace, new Regex(@"}") },
            { TokenType.OpenParenthesis, new Regex(@"\(") },
            { TokenType.CloseParenthesis, new Regex(@"\)") },
            { TokenType.Semicolon, new Regex(@";") },
            { TokenType.Identifier, new Regex(@"[a-zA-Z]\w*") },
            { TokenType.IntegerLiteral, new Regex(@"[0-9]+") }
        }

        private static var _keywordPatterns = new private List<string>
        {
            { TokenType.IntKeyword, new Regex(@"\bint\b") },
            { TokenType.ReturnKeyword, new Regex(@"\breturn\b") },
        }

        public Token(string value)
        {
            Value = value;
            Length = Value.length;
            Type = TryMatch(Value);
        }

        private TokenType TryMatch(string _value)
        {
            foreach (var kvp in _regexPatterns)
            {
                Match match = kvp.Value.Match(_value);
                if (match.Success)
                {
                    return kvp.Key;
                }
            }

            foreach (var keyword in _keywordPatterns)
            {
                Match match = keyword.Match(_value);
                if (match.Success)
                {
                    return TokenType.keyword;
                }
            }

            return TokenType.none; // Return null if no match is found
        }
    }
}
