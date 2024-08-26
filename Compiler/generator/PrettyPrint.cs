using System.Collections;
using Tree;

namespace Generate
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

        public string Visit(CProgram program) => $"{program.function.Accept(this)}";

        public string Visit(Function function)
        {
            return $"\nFUNCTION {function.identifier}:{function.statement.Accept(this)}";
        }

        public string Visit(Statement statement) => throw new Exception("Cannot visit generic statement");

        public string Visit(ReturnStatement returnStatement)
        {
            return $"\n\tRETURN {returnStatement.expression.Accept(this)}";
        }
        
        public string Visit(VariableDeclaration variableDeclaration)
        {
            return $"\n\tSET {variableDeclaration.type} {variableDeclaration.identifier} = {variableDeclaration.value}";
        }

        public string Visit(Expression expression) => throw new Exception("Cannot visit generic expression");

        public string Visit(UnaryOperation unaryOperation)
        {
            return $"{unaryOperation.unaryOperator}<{unaryOperation.expression.Accept(this)}>";
        }

        public string Visit(Constant constant)
        {
            return $"INT<{constant.integerLiteral}>";
        }

    }
}