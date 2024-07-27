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

        protected bool CheckTokenValue(string[] values)
        {
            if (!tokens.MoveNext()) return false;
            foreach (string value in values)
            {
                if (tokens.Current.Value == value) return true;
            }
            return false;
        }
        
        protected bool CheckTokenType(TokenType tType)
        {
            return tokens.MoveNext() && tokens.Current.Type == tType;
        }
        
        protected bool CheckTokenType(TokenType[] tTypes)
        {
            if (!tokens.MoveNext()) return false;
            foreach (TokenType tType in tTypes)
            {
                if (tokens.Current.Type == tType) return true;
            }
            return false;
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
            var tokensCopy = tokens.Clone();
            UnaryOperator? unaryOperation = new UnaryOperatorParser(tokens).Parse();
            Expression? expression = new ExpressionParser(tokens).Parse();
            if (unaryOperation is not null && expression is not null) 
            {
                return new Expression(unaryOperation, expression);
            }
            
            Constant? constant = new ConstantParser(tokens).Parse();
            if (constant is not null) return new Expression(constant);

            tokens = tokensCopy;
            return null;
        }
    }

    public class UnaryOperatorParser : Parser
    {
        TokenType[] operators = [
            TokenType.bitwiseComplement,
            TokenType.negation,
            TokenType.logicalNegation
        ];

        public UnaryOperatorParser(SmartList<Token> _token) : base(_token)
        {
        }

        public override UnaryOperator? Parse()
        {
            var tokensCopy = tokens.Clone();
            if (!CheckTokenType(operators)) return new UnaryOperator(tokens.Current.Type);
            tokens = tokensCopy;
            return null;
        }
    }

    public class ConstantParser : Parser
    {
        public ConstantParser(SmartList<Token> _token) : base(_token)
        {
        }

        public override Constant? Parse()
        {
            var tokensCopy = tokens.Clone();
            if (CheckTokenType(TokenType.integerLiteral)) return new Constant(tokens.Current.Value);
            tokens = tokensCopy;
            return null;
        }
    }
}
    
