using CompilerUtility;
using Lex;

namespace Parse
{
    public abstract class Parser
    {
        protected SmartList<Token> tokens;
        public Parser(SmartList<Token> _tokens)
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


    public class FunctionParser : Parser
    {
        public FunctionParser(SmartList<Token> _token) : base(_token)
        {
        }
        
        public override Function? Parse()
        {

            bool sequenceCheck = CheckSequence([
                CheckTokenValue("int"),
                CheckTokenType(TokenType.identifier),
            ]);
            Console.WriteLine(tokens.Current.Value);
            if (!sequenceCheck) return null;
            
                string identifier = tokens.Current.Value;

            sequenceCheck = CheckSequence([
                CheckTokenType(TokenType.openParantheses),
                CheckTokenType(TokenType.closeParentheses),
                CheckTokenType(TokenType.openBrace)
            ]);
            if (!sequenceCheck) return null;
            
            Statement? statement = new StatementParser(tokens).Parse();
            if (statement is null) return null;

            if (!CheckTokenType(TokenType.closeBrace)) return null;
            
            return new Function(identifier, statement);
        }
    
    }

    public class StatementParser : Parser
    {
        public StatementParser(SmartList<Token> _token) : base(_token)
        {
        }

        public override Statement? Parse()
        {
            Return? returnStatement = new ReturnParser(tokens).Parse();
            if (returnStatement is null) return null;

            return new Statement(returnStatement);
        }
    }

    public class ReturnParser : Parser
    {
        public ReturnParser(SmartList<Token> _token) : base(_token)
        {
        }

        public override Return? Parse()
        {
            if (!CheckTokenValue("return")) return null;
            
            Expression? expression = new ExpressionParser(tokens).Parse();
            if (expression is null) return null;
            
            if (!CheckTokenType(TokenType.semicolon)) return null;

            return new Return(expression);
        }
    }

    public class ExpressionParser : Parser
    {
        public ExpressionParser(SmartList<Token> _token) : base(_token)
        {
        }

        public override Expression? Parse()
        {
            Constant? constant = new ConstantParser(tokens).Parse();
            if (constant is null) return null;
            return new Expression(constant);

        }
    }

    public class ConstantParser : Parser
    {
        public ConstantParser(SmartList<Token> _token) : base(_token)
        {
        }

        public override Constant? Parse()
        {
            if (!CheckTokenType(TokenType.integerLiteral)) return null;            
            return new Constant(tokens.Current.Value);
        }
    }
}
    
