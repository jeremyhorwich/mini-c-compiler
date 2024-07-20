using System.Data;
using Lex;

namespace Parser
{
    abstract class Node
    {
        public bool parseValid;

        protected int _tokensUsed = 0;
        public int tokensUsed {get 
        {
            if (_tokensUsed == 0)
            {
                throw new DataException("Tried to access length for invalid parse");
            }
            return _tokensUsed;
        }}

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
                _tokensUsed = 1;
                return true;                
            }

            return false;
        }

        protected override string GenerateImplementation()
        {
            return $"\n mov1    ${integerLiteral}, %eax";
        }

        public override string ToString()
        {
            return $"Int<{integerLiteral}>";
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
            
            expression = new Expression(tokens.GetRange(1,1));
            if (!expression.parseValid) return false;
            if (tokens[2].Type != TokenType.semicolon) return false;

            _tokensUsed = 2 + expression.tokensUsed;
            return true;
        }

        protected override string GenerateImplementation()
        {
            string assembly = "";
            assembly += expression?.Generate();
            assembly += "\n ret";
            return assembly;
        }

        public override string ToString()
        {
            return $"\n     RETURN {expression?.ToString()}";
        }
    }


    class Function : Node
    {
        string? identifier;
        Return? _ret;

        public Function(List<Token> tokens) : base(tokens)
        {
        }

        public override bool Parse(List<Token> tokens)
        {
            if (tokens.Count < 7) return false;
            if (tokens[0].Value != "int") return false;
            if (tokens[1].Type != TokenType.identifier) return false;
            
            identifier = tokens[1].Value;

            if (tokens[2].Type != TokenType.openParantheses) return false;
            if (tokens[3].Type != TokenType.closedParentheses) return false;
            if (tokens[4].Type != TokenType.openBrace) return false;

            _ret = new Return(tokens.GetRange(5,tokens.Count - 5));
            if (!_ret.parseValid) return false;

            if (tokens[5 + _ret.tokensUsed].Type != TokenType.closeBrace) return false;
        
            _tokensUsed = _ret.tokensUsed + 6;
            return true;
        }

        protected override string GenerateImplementation()
        {
            string assembly = "";
            assembly += $" .globl _{identifier} \n_{identifier}:";
            assembly += _ret?.Generate();
            return assembly;
        }

        public override string ToString()
        {
            string declaration = $"FUN INT {identifier}";
            string parameters = "\n Params: ()";
            string body = $"\n Body: {_ret?.ToString()}";
            
            return(declaration + parameters + body);
        }
    }
}