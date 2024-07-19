using Lex;

namespace Parser
{
    abstract class Node
    {
        public bool parseValid;
        public List<Token> validTokens = new List<Token>();

        public Node(List<Token> tokens)
        {
            validTokens = Parse(tokens);
        }

        public abstract List<Token> Parse(List<Token> tokens);
        public abstract string Generate();
    }


    class Expression : Node
    {
        public Expression(List<Token> tokens) : base(tokens)
        {
        }

        public override List<Token> Parse(List<Token> tokens)
        {
            if (tokens[0].Type == TokenType.integerLiteral)
            {
                return [tokens[0]];
                //Must change to match regular expressions (integers of more than one digit)
            }

            return [];
        }

        public override string Generate()
        {
            return "";
        }
    }   

    class Return : Node
    {
        public Return(List<Token> tokens) : base(tokens)
        {
        }

        public override List<Token> Parse(List<Token> tokens)
        {
            if (tokens.Count < 3)
            {
                return [];
            }

            if (tokens[0].Value != "return")
            {
                return [];
            }

            if (tokens[1].Type != TokenType.integerLiteral) 
            {
                return [];
                //Must modify indices for multidigit integers
            }
            
            if (tokens[2].Type != TokenType.semicolon)
            {
                return [];
            }

            return tokens.GetRange(0, 3);
        }

        public override string Generate()
        {
            return "";
        }
    }
}