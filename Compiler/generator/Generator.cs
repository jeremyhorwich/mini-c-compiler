using Tree;

namespace Generate
{
    public class Generator : IVisitor<string>
    {
        private CProgram program;
        private CEnvironment environment;

        public Generator(CProgram _program)
        {
            program = _program;
            environment = new CEnvironment();
        }

        public string Generate()
        {
            return program.Accept(this);
        }
        
        
        public string Visit(CProgram program)
        {
            string assembly = "";
            assembly += program.function.Accept(this);
            return assembly;
        }

        public string Visit(Function function)
        {
            environment = environment.Wrap();
            string assembly = $"\n{function.identifier}:"; 
            assembly += function.statement.Accept(this);
            environment = environment.Unwrap();
            return assembly;
        }

        public string Visit(Statement statement) => throw new Exception("Cannot visit generic statement");

        public string Visit(ReturnStatement returnStatement)
        {
            string assembly = "";
            assembly += returnStatement.expression.Accept(this);
            assembly += new Instruction("ret", "").Format();
            return assembly;
        }

        public string Visit(VariableDeclaration variableDeclaration)
        {
            environment.Define(variableDeclaration.identifier, variableDeclaration.value);
            CVariable defined = environment.Get(variableDeclaration.identifier);
            return new Instruction("movl", $"${defined.value}, {defined.address}").Format();
        }

        public string Visit(Expression expression) => throw new Exception("Cannot visit generic expression");

        public string Visit(Constant constant)
        {
            string assembly = "";
            assembly += new Instruction($"movl", $"${constant.integerLiteral}, %eax").Format();
            return assembly;
        }
    }
}