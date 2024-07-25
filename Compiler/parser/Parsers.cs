using Lex;

namespace Parse
{
    public static class Parsers
    {
        public static Function? ParseFunction(IEnumerator<Token> tokens)
        {
            if (!tokens.MoveNext() || tokens.Current.Value != "int") return null;
            
            if (!tokens.MoveNext() || tokens.Current.Type == TokenType.identifier) return null;
            string identifier = tokens.Current.Value;

            if (!tokens.MoveNext() || tokens.Current.Type != TokenType.openParantheses) return null;
            if (!tokens.MoveNext() || tokens.Current.Type != TokenType.closedParentheses) return null;
            if (!tokens.MoveNext() || tokens.Current.Type != TokenType.openBrace) return null;
            
            Statement? statement = ParseStatement(tokens);
            if (statement is null) return null;

            if (!tokens.MoveNext() || tokens.Current.Type != TokenType.closeBrace) return null;
            
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
    
