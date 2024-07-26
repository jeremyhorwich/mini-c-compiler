using System.Net.Http.Headers;

namespace Parse
{
    public abstract class Node
    {
        public abstract string Generate();
    }


    public class Constant : Node
    {
        private string integerLiteral;
        public Constant(string _integerLiteral)
        {
            integerLiteral = _integerLiteral;
        }

        public override string Generate()
        {
            return new Instruction("movl", $"${integerLiteral}, %eax").Format();
        }

        public override string ToString()
        {
            return $"Int<{integerLiteral}>";
        }
    }

    public class Expression : Node
    {
        private Constant? constant;
        public Expression(Constant _constant)
        {
            constant = _constant;
        }

        public override string Generate()
        {
            if (constant is not null)
            {
                return constant.Generate();
            }
            
            return "";  //this make sense for when we have multiple ways of declaring an expression
        }

        public override string ToString()
        {
            return $"{constant}";
        }
    }

    public class Return : Node
    {
        private Expression expression;

        public Return(Expression _expression)
        {
            expression = _expression;
        }

        public override string Generate()
        {
            string assembly = "";
            assembly += expression.Generate();
            assembly += new Instruction("popq", "%rbp").Format();
            assembly += new Instruction("ret", "").Format();
            return assembly;
        }

        public override string ToString()
        {
            return $"\n\tRETURN {expression}";
        }
    }

    public class Statement : Node
    {
        private Return? returnStatement;
        public Statement(Return _returnStatement)
        {
            returnStatement = _returnStatement;
        }

        public override string Generate()
        {
            if (returnStatement is not null)
            {
                return returnStatement.Generate();
            }
            
            return "";  //this make sense for when we have multiple ways of declaring an expression
        }

        public override string ToString()
        {
            if (returnStatement is not null)
            {
                return returnStatement.ToString(); 
            }
            return "";
        }
    }

    public class Function : Node
    {
        private string identifier;
        Statement statement;

        public Function(string _identifier, Statement _Statement)
        {
            identifier = _identifier;
            statement = _Statement;
        }

        public override string Generate()
        {
            string assembly = "";
            assembly += new Instruction(".globl",identifier).Format();
            assembly += $"\n{identifier}:";
            assembly += new Instruction("pushq", "%rbp").Format();
            assembly += new Instruction("movq", "%rsp, %rbp").Format();
            assembly += statement.Generate();
            return assembly;
        }

        public override string ToString()
        {
            string declaration = $"FUN INT {identifier}";
            string parameters = "\n Params: ()";
            string body = $"\n Body: {statement}";
            
            return declaration + parameters + body;
        }
    }
}