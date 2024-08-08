using Tree;
using Lex;

namespace Parse
{
    public class ParseException : SystemException 
    {
        public int Line { get; }
        public int LocalLine { get; }
        public string CurrentFunction { get; }
        
        public ParseException(string message, int line, int localLine, string currentFunction) 
            : base(FormatMessage(message, line, localLine, currentFunction))
        {
            Line = line;
            LocalLine = localLine;
            CurrentFunction = currentFunction;
        }

        private static string FormatMessage(string message, int line, int localLine, string currentFunction)
        {
            return $"Error: line {line} in function {currentFunction}, line {localLine} - {message}";
        }
    }
    
    public class Parser
    {
        private List<Token> Tokens;
        private int Current = 0;
        public bool hadError { get; private set; } = false;

        private int Line = 1;
        private int LocalLine = 1;
        private string CurrentFunction = "";
        public Parser(List<Token> _tokens)
        {
            Tokens = _tokens;
        }

        private bool Match(TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
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
                if (Check(value))
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
            if (!Check(type))
            {
                hadError = true;
                throw new ParseException(message, Line, LocalLine, CurrentFunction);
            }
            if (Check(TokenType.newLine))
            {
                Line++;
                LocalLine++;
                Advance();
                return Consume(type, message);
            }
            return Advance();
        }

        private Token Consume(string value, string message)
        {
            if (!Check(value))
            {
                hadError = true;
                throw new ParseException(message, Line, LocalLine, CurrentFunction);
            }
            if (Check(TokenType.newLine))
            {
                Line++;
                LocalLine++;
                Advance();
                return Consume(value, message);
            }
            return Advance();
        }

        private Token Previous()
        {
            return Tokens[Current - 1];
        }

        private void Synchronize()
        {
            Advance();
            
            while (!EndReached()) 
            {
                if (Previous().Type == TokenType.semicolon) return;
                
                switch (Peek().Value) 
                {
                    case "return":
                        return;
                }
                
                Advance();
            }
      }

        public CProgram Parse()
        {
            try
            {
                Function function = MatchFunction();
                return new CProgram(function);
            }
            catch (ParseException)
            {
                throw;      //TOOD refactor to loop over list of functions and try each separately
            }
        }

        private Function MatchFunction()
        {
            try
            {
                Consume("int", "Expected int");

                string identifier = Consume(TokenType.identifier, "Expected identifier after int").Value;
                
                LocalLine = 1;
                CurrentFunction = identifier;

                Consume(TokenType.openParantheses, "Expected ( after" + identifier);
                Consume(TokenType.closeParentheses, "Expected ) after (");
                Consume(TokenType.openBrace, "Expected { after )");

                Statement statement = MatchStatement();
                
                Consume(TokenType.closeBrace, "Expected } at function end");

                return new Function(identifier, statement);
            }
            catch (ParseException e)
            {
                Console.WriteLine(e.Message);
                Synchronize();
                return new DefaultFunction("ERROR");
            }
        }

        private Statement MatchStatement()
        {
            try
            {
                if (Match(["return"]))
                {
                    return MatchReturnStatement();
                }
                hadError = true;
                throw new ParseException("No valid statement found", Line, LocalLine, CurrentFunction);
            }
            catch (ParseException e)
            {
                Console.WriteLine(e.Message);
                Synchronize();
                return new DefaultStatement();
            }
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
            hadError = true;
            throw new ParseException("No valid expression found",  Line, LocalLine, CurrentFunction);
        }
    }
}
    
