using Lexer;
using System.Text.RegularExpressions;

namespace Lexer 
{
    public enum TokenType
    {
        keyword,
        openParantheses,
        closedParentheses,
        openBrace,
        closeBrace,
        integerLiteral,
        semicolon,
        identifier,
        none
    }

    public class Token
    {

        public TokenType Type { get; set; }
        public string Value { get; set; }
        public int Length => Value.Length;

        private static Dictionary<TokenType, Regex> _regexPatterns = new Dictionary<TokenType, Regex>
        {
            { TokenType.openBrace, new Regex(@"{") },
            { TokenType.closeBrace, new Regex(@"}") },
            { TokenType.openParantheses, new Regex(@"\(") },
            { TokenType.closedParentheses, new Regex(@"\)") },
            { TokenType.semicolon, new Regex(@";") },
            { TokenType.identifier, new Regex(@"[a-zA-Z]\w*") },
            { TokenType.integerLiteral, new Regex(@"[0-9]+") }
        };

        private static List<Regex> _keywordPatterns = new List<Regex>
        {
            new Regex(@"\bint\b"),
            new Regex(@"\breturn\b")
        };

        public Token(string value)
        {
            Value = value;
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
