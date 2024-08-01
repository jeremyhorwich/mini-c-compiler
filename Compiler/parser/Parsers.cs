using CompilerUtility;
using Tree;
using Lex;

namespace Parse
{
    public class Parser
    {
        private List<Token> Tokens;
        private int Current = 0;
        public Parser(List<Token> _tokens)
        {
            Tokens = _tokens;
        }

        private bool Match(TokenType[] types)
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

        private bool Match(string[] values)
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

        private bool Check(TokenType type)
        {
            if (EndReached()) return false;
            return Tokens[Current].Type == type;
        }

        private bool Check(string value)
        {
            if (EndReached()) return false;
            return Tokens[Current].Value == value;
        }

        private bool EndReached()
        {
            return Peek().Type == TokenType.endOfFile;
        }
        
        private Token Peek()
        {
            return Tokens[Current];
        }

        private Token Advance()
        {
            if (!EndReached()) Current++;
            return Previous();
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw new Exception(message);    //TODO
        }

        private Token Consume(string value, string message)
        {
            if (Check(value)) return Advance();
            throw new Exception(message);   //TODO
        }

        private Token Previous()
        {
            return Tokens[Current - 1];
        } 

        public Program Parse()
        {
            try
            {
                Function function = MatchFunction();
                return new Program(function);
            }
            catch   //TODO
            {
                return null;
            }
        }

        private Function MatchFunction()
        {
            Consume("int", "Expected int");

            string identifier = Consume(TokenType.identifier, "Expected identifier after int").Value;

            Consume(TokenType.openParantheses, "Expected ( after" + identifier);
            Consume(TokenType.closeParentheses, "Expected ) after (");
            Consume(TokenType.openBrace, "Expected { after )");

            Statement statement = MatchStatement();
            
            Consume(TokenType.closeBrace, "Expected } at function end");

            return new Function(identifier, statement);
        }

        private Statement MatchStatement()
        {
            if (Match(["return"])) return MatchReturnStatement();
            return null;
        }

        private Statement MatchReturnStatement()
        {
            Expression expression = MatchExpression();
            Consume(TokenType.semicolon, "Expected semicolon at end of statement");
            return new ReturnStatement(expression);
        }

        private Expression MatchExpression()
        {
            if (Match([TokenType.integerLiteral]))
            {
                return new Constant(Previous().Value);
            }
            return null;    //TODO throw error
        }
    }
}
    
