using CompilerUtility;
using Lex;

namespace Parse
{
    public class Parser
    {
        protected List<Token> Tokens;
        protected int Current = 0;
        public Parser(List<Token> _tokens)
        {
            Tokens = _tokens;
        }

        protected bool Match(TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (!Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        protected bool Match(string[] values)
        {
            foreach (string value in values)
            {
                if (!Check(value))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        protected bool Check(TokenType type)
        {
            if (EndReached()) return false;
            return Tokens[Current].Type == type;
        }

        protected bool Check(string value)
        {
            if (EndReached()) return false;
            return Tokens[Current].Value == value;
        }

        protected bool EndReached()
        {
            return Peek().Type == TokenType.endOfFile;
        }
        
        protected Token Peek()
        {
            return Tokens[Current];
        }

        protected Token Advance()
        {
            if (!EndReached()) Current++;
            return Previous();
        }

        protected Token Previous()
        {
            return Tokens[Current - 1];
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
            Console.WriteLine(Tokens.Current.Value);
            if (!sequenceCheck) return null;
            
                string identifier = Tokens.Current.Value;

            sequenceCheck = CheckSequence([
                CheckTokenType(TokenType.openParantheses),
                CheckTokenType(TokenType.closeParentheses),
                CheckTokenType(TokenType.openBrace)
            ]);
            if (!sequenceCheck) return null;
            
            Statement? statement = new StatementParser(Tokens).Parse();
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
            Return? returnStatement = new ReturnParser(Tokens).Parse();
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
            
            Expression? expression = new ExpressionParser(Tokens).Parse();
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
            Tokens.SaveState();
            UnaryOperator? unaryOperation = new UnaryOperatorParser(Tokens).Parse();
            Expression? expression = new ExpressionParser(Tokens).Parse();
            if (unaryOperation is not null && expression is not null) 
            {
                Tokens.DeleteState();
                return new Expression(unaryOperation, expression);
            }
            
            Constant? constant = new ConstantParser(Tokens).Parse();
            if (constant is not null)
            {
                Tokens.DeleteState();
                return new Expression(constant);
            }
            Tokens.RestoreState();
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
            Tokens.SaveState();
            if (!CheckTokenType(operators)) 
            {
                Tokens.DeleteState();
                return new UnaryOperator(Tokens.Current.Type);
            }    
            Tokens.RestoreState();
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
            Tokens.SaveState();
            if (CheckTokenType(TokenType.integerLiteral)) 
            {
                Tokens.DeleteState();
                return new Constant(Tokens.Current.Value);
            }
            Tokens.RestoreState();
            return null;
        }
    }
}
    
