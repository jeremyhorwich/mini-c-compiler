using Lex;
using System.Collections.Generic;

namespace Parse
{
    public abstract class Parser
    {
        protected IEnumerator<Token> tokens;
        public Parser(IEnumerator<Token> _tokens)
        {
            tokens = _tokens;
        }

        public abstract Node? Parse();
        protected bool CheckTokenValue(string value)
        {
            return tokens.MoveNext() && tokens.Current.Value == value;
        }
        
        protected bool CheckTokenType(TokenType tType)
        {

            return tokens.MoveNext() && tokens.Current.Type == tType;
        }

        protected bool CheckSequence(bool[] parseConditions)
        {
            foreach(var condition in parseConditions)
            {
                if (!condition) return false;
            }
            return true;
        }
    }

    public class ConstantParser : Parser
    {
        public ConstantParser(IEnumerator<Token> _token) : base(_token)
        {
        }

        public override Constant? Parse()
        {
            if (!CheckTokenType(TokenType.integerLiteral)) return null;            
            return new Constant(tokens.Current.Value);
        }
    }

    public class FunctionParser : Parser
    {
        public FunctionParser(IEnumerator<Token> _token) : base(_token)
        {
        }
        
        public override Function? Parse()
        {
            bool sequenceCheck = CheckSequence([
                CheckTokenValue("int"),
                CheckTokenType(TokenType.identifier),
            ]);
            if (!sequenceCheck) return null;
            
                string identifier = tokens.Current.Value;

            sequenceCheck = CheckSequence([
                CheckTokenType(TokenType.openParantheses),
                CheckTokenType(TokenType.closedParentheses),
                CheckTokenType(TokenType.openBrace)
            ]);
            if (!sequenceCheck) return null;
            
            Statement? statement = ParseStatement(tokens);
            if (statement is null) return null;

            if (!CheckTokenType(TokenType.closeBrace)) return null;
            
            return new Function(identifier, statement);
        }

    }

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
    
