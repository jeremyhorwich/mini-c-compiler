using System.Text.RegularExpressions;

namespace Lex 
{
    public enum TokenType
    {
        keyword,
        openParantheses,
        closeParentheses,
        openBrace,
        closeBrace,
        integerLiteral,
        semicolon,
        identifier,
        negation,
        bitwiseComplement,
        logicalNegation,
        none
    }

    public class Token
    {

        private string _value {get; set;}
        public string Value => _value;
        public int Length => Value.Length;
        private TokenType _type;
        public TokenType Type => _type;

        private static Dictionary<TokenType, Regex> _regexPatterns = new Dictionary<TokenType, Regex>
        {
            { TokenType.openBrace, new Regex(@"^{") },
            { TokenType.closeBrace, new Regex(@"^}") },
            { TokenType.openParantheses, new Regex(@"^\(") },
            { TokenType.closeParentheses, new Regex(@"^\)") },
            { TokenType.semicolon, new Regex(@"^;") },
            { TokenType.identifier, new Regex(@"^[a-zA-Z]\w*") },
            { TokenType.integerLiteral, new Regex(@"^[0-9]+") },
            { TokenType.negation, new Regex(@"^-") },
            { TokenType.bitwiseComplement, new Regex(@"^~") },
            { TokenType.logicalNegation, new Regex(@"^!") }
        };

        private static List<Regex> _keywordPatterns = new List<Regex>
        {
            new Regex(@"^\bint\b"),
            new Regex(@"^\breturn\b")
        };

        public Token(string input)
        {
            _value = " ";
            FindMatch(input);
        }

        public override string ToString()
        {
            return Value;
        }

        private void FindMatch(string input)
        {
            foreach (var keyword in _keywordPatterns)
            {
                Match match = keyword.Match(input);
                if (match.Success)
                {
                    _type = TokenType.keyword;
                    _value = match.Value;
                    return;
                }
            }
                        
            foreach (var kvp in _regexPatterns)
            {
                Match match = kvp.Value.Match(input);
                if (match.Success)
                {
                    _type = kvp.Key;
                    _value = match.Value;
                    return;
                }
            }

            _type = TokenType.none;
        }
    }
}
