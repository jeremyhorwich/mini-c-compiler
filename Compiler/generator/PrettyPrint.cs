using Tree;

namespace Generator
{
    class PrettyPrinter : IVisitor<string>
    {
        public CProgram program;
        public PrettyPrinter(CProgram _program)
        {
            program = _program;
        }

        public void Display()
        {
            Console.WriteLine(program.Accept(this));
        }

        public string Visit(CProgram program) => $"{Visit(program.function)}";

        public string Visit(Function function)
        {
            return $"\nFUNCTION {function.identifier}:{Visit(function.statement)}";
        }

        public string Visit(Statement statement)
        {
            return statement.Accept(this);
        }

        public string Visit(ReturnStatement returnStatement)
        {
            return $"\n\tRETURN {Visit(returnStatement.expression)}";
        }

        public string Visit(Expression expression)
        {
            return expression.Accept(this);
        }

        public string Visit(Constant constant)
        {
            return $"INT<{constant.integerLiteral}>";
        }

    }
}