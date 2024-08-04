using Tree;

namespace Generator
{
    class PrettyPrinter : IVisitor<string>
    {
        public string Visit(Program program) => $"\n{Visit(program.function)}";

        public string Visit(Function function)
        {
            return $"\n\t{function.identifier}{Visit(function.statement)}";
        }

        public string Visit(Statement statement) => "";

        public string Visit(ReturnStatement returnStatement)
        {
            return $"\n\tRETURN {returnStatement.expression}";
        }

        public string Visit(Expression expression) => "";

        public string Visit(Constant constant)
        {
            return $"INT<{constant.integerLiteral}>";
        }

    }

    class Test
    {
        public void test()
        {
            var pprint = new PrettyPrinter();
            var constant = new Constant("5");
        }
    }
}