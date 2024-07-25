using Lex;

namespace Parse
{
    public static class Parsers
    {
        private static bool CheckTokenValue(IEnumerator<Token> tokens, string value)
        {
            return !tokens.MoveNext() || tokens.Current.Value == value;
        }
        
        private static bool CheckTokenType(IEnumerator<Token> tokens, TokenType tType)
        {

            return !tokens.MoveNext() || tokens.Current.Type == tType;
        }

        public static Function? ParseFunction(IEnumerator<Token> tokens)
        {
            if (!CheckTokenValue(tokens, "int")) return null;
            if (!CheckTokenType(tokens, TokenType.identifier)) return null;
            string identifier = tokens.Current.Value;

            if (!CheckTokenType(tokens, TokenType.openParantheses)) return null;
            if (!CheckTokenType(tokens, TokenType.closedParentheses)) return null;
            if (!CheckTokenType(tokens, TokenType.openBrace)) return null;
           
            Statement? statement = ParseStatement(tokens);
            if (statement is null) return null;

            if (!CheckTokenType(tokens, TokenType.closeBrace)) return null;
            
            return new Function(identifier, statement);
        }

        public static Statement? ParseStatement(IEnumerator<Token> tokens)
        {
            Return? returnStatement = ParseReturn(tokens);
            if (returnStatement is null) return null; 

            return new Statement(returnStatement);
        }

        public static Return? ParseReturn(IEnumerator<Token> tokens)
        {
            if (!tokens.MoveNext() || tokens.Current.Value != "return") return null;

            Expression? expression = ParseExpression(tokens);
            if (expression is null) return null;

            if (!tokens.MoveNext() || tokens.Current.Type != TokenType.semicolon) return null;
            
            return new Return(expression);
        }

        public static Expression? ParseExpression(IEnumerator<Token> tokens)
        {
            if (!tokens.MoveNext()) return null;
            Constant? constant = ParseConstant(tokens);
            if (constant is null) return null;
            
            return new Expression(constant);
        }

        public static Constant? ParseConstant(IEnumerator<Token> tokens)
        {
            if (!tokens.MoveNext() || tokens.Current.Type != TokenType.integerLiteral) return null;            
            return new Constant(tokens.Current.Value);
        }

    }
}
    
