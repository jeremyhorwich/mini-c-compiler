using Lex;

namespace Parse
{
    public static class Parsers
    {
        public delegate Node? Parser(IEnumerator<Token> tokens);

        public static Node? ParseFunction(IEnumerator<Token> tokens)
        {
            if (!tokens.MoveNext()) return null;
            if (tokens.Current.Value != "int") return null;
            if (!tokens.MoveNext()) return null;
            if (tokens.Current.Type == TokenType.identifier) return null;

            string identifier = tokens.Current.Value;

            if (!tokens.MoveNext()) return null;
            if (tokens.Current.Type != TokenType.openParantheses) return null;
            if (!tokens.MoveNext()) return null;
            if (tokens.Current.Type != TokenType.closedParentheses) return null;
            if (!tokens.MoveNext()) return null;
            if (tokens.Current.Type != TokenType.openBrace) return null;
            if (!tokens.MoveNext()) return null;
            
            Statement statement = ParseStatement(tokens);
            if (statement is null) return null;

            if (!tokens.MoveNext()) return null;
            if (tokens.Current.Type != TokenType.closeBrace) return null;
            
            return new Function(identifier, statement);
        }

        public static Node? ParseStatement(IEnumerator<Token> tokens)
        {
            return new Statement()
        }

        public static Node? ParseReturn(IEnumerator<Token> tokens)
        {
            if (tokens.Count < 3) return null;
            if (tokens[0].Value != "return") return null;
            
            expression = new Expression(tokens.GetRange(1,1));
            if (!expression.parseValid) return null;
            if (tokens[2].Type != TokenType.semicolon) return null;

            _tokensUsed = 2 + expression.tokensUsed;
            return true;
        }
        public static Node? ParseConstant(IEnumerator<Token> tokens)
        {
            if (tokens.Count != 1) return null;
            if (tokens[0].Type != TokenType.integerLiteral) return null;
            
            return new Constant(tokens[0].Value);
        }

    }
}
    
