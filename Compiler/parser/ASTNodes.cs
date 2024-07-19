using Lex;

namespace Parser
{
    abstract class Node
    {
        public bool parseValid;

        public Node(List<Token> tokens)
        {
            parseValid = Parse(tokens);
        }

        public abstract bool Parse(List<Token> tokens);
        protected abstract string GenerateImplementation();
        public string Generate()
        {
            if (!parseValid)
            {
                throw new InvalidOperationException("Tried to generate string for invalid parse");
            }

            return GenerateImplementation();
        }
    }


    class Expression : Node
    {
        private string? integerLiteral;
        public Expression(List<Token> tokens) : base(tokens)
        {
        }

        public override bool Parse(List<Token> tokens)
        {
            if (tokens[0].Type == TokenType.integerLiteral)
            {
                integerLiteral = tokens[0].Value;
                return true;                
            }

            return false;
        }

        protected override string GenerateImplementation()
        {
            return $"mov1    ${integerLiteral}, %eax";
        }
    }   

    class Return : Node
    {
        private Expression? expression;

        public Return(List<Token> tokens) : base(tokens)
        {
        }


        public override bool Parse(List<Token> tokens)
        {
            if (tokens.Count < 3) return false;
            if (tokens[0].Value != "return") return false;
            if (tokens[1].Type != TokenType.integerLiteral) return false;
            if (tokens[2].Type != TokenType.semicolon) return false;

            expression = new Expression(tokens.GetRange(1,2));
            return true;
        }

        protected override string GenerateImplementation()
        {
            string assembly = "";
            assembly += expression?.Generate();
            assembly += "\nret";
            return assembly;
        }
    }
}