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
        equalSign,
        identifier,
        negation,
        bitwiseComplement,
        logicalNegation,
        newLine,
        whiteSpace,
        endOfFile,
        none
    }

    public class Token
    {

        private string _value {get; set;}
        public string Value => _value;
        public int Length => Value.Length;
        private TokenType _type;
        public TokenType Type => _type;

        private static Dictionary<Regex, TokenType> _regexPatterns = new Dictionary<Regex, TokenType>
        {
            { new Regex(@"^{"), TokenType.openBrace },
            { new Regex(@"^}"), TokenType.closeBrace },
            { new Regex(@"^\("), TokenType.openParantheses },
            { new Regex(@"^\)"), TokenType.closeParentheses },
            { new Regex(@"^;"), TokenType.semicolon },
            { new Regex(@"^="), TokenType.equalSign},
            { new Regex(@"^[a-zA-Z]\w*"), TokenType.identifier },
            { new Regex(@"^[0-9]+"), TokenType.integerLiteral },
            { new Regex(@"^-"), TokenType.negation },
            { new Regex(@"^~"), TokenType.bitwiseComplement },
            { new Regex(@"^!"), TokenType.logicalNegation },
            { new Regex(@"^\r\n|\n"), TokenType.newLine },
            { new Regex(@"^\s+"), TokenType.whiteSpace },
            { new Regex(@"^//.*$"), TokenType.whiteSpace },     //Single-line comments
            { new Regex(@"/\*[\s\S]*?\*/"), TokenType.whiteSpace }      //Multi-line comments
        };
        //TODO string literals (finite state machine?)

        private static List<Regex> _keywordPatterns = new List<Regex>
        {
            new Regex(@"^\bint\b"),
            new Regex(@"^\breturn\b")
        };

        public Token(string input)
        {
            _value = " ";       //Space instead of empty string because Length depends on Value
            FindMatch(input);
        }

        public Token(TokenType tokenType)
        {
            _value = " ";
            _type = tokenType;
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
                Match match = kvp.Key.Match(input);
                if (match.Success)
                {
                    _type = kvp.Value;
                    _value = match.Value;
                    return;
                }
            }

            _type = TokenType.none;
        }
    }
}
