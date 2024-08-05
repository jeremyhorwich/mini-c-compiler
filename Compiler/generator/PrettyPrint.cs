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
            Console.WriteLine(Visit(program));;
        }

        public string Visit(CProgram program) => $"{Visit(program.function)}";

        public string Visit(Function function)
        {
            return $"\nFUNCTION {function.identifier}:{Visit(function.statement)}";
        }

        public string Visit(Statement statement)
        {
            switch (statement)
            {
                case ReturnStatement returnStmt:
                    return Visit(returnStmt);
                default:
                    throw new InvalidOperationException($"Unhandled statement type: {statement.GetType().Name}");
            }
        }

        public string Visit(ReturnStatement returnStatement)
        {
            return $"\n\tRETURN {Visit(returnStatement.expression)}";
        }

        public string Visit(Expression expression)
        {
            switch (expression)
            {
                case Constant constant:
                    return Visit(constant);
                default:
                    throw new InvalidOperationException($"Unhandled expression type: {expression.GetType().Name}");
            }
        }

        public string Visit(Constant constant)
        {
            return $"INT<{constant.integerLiteral}>";
        }

    }
}