namespace Tree
{
    public interface IVisitor<T>
    {
        T Visit(CProgram program);
        T Visit(Function function);
        T Visit(Statement statement);
        T Visit(ReturnStatement returnStatement);
        T Visit(Expression expression);
        T Visit(Constant constant);
    }   

    public abstract class Node
    {
    }

    public class CProgram : Node
    {
        public Function function;
        public CProgram(Function _function)
        {
            function = _function;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Function : Node 
    {
        public string identifier;
        public Statement statement;

        public Function(string _identifier, Statement _statement)
        {
            identifier = _identifier;
            statement = _statement;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class DefaultFunction : Function
    {
        public DefaultFunction(string _identifier) : base(_identifier, null!)
        {
            identifier = _identifier;
        }

        public new Statement statement {get {return null!; }}
    }

    public abstract class Statement : Node
    {
        public virtual T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class DefaultStatement : Statement
    {
        public DefaultStatement() : base()
        {
        }
    }

    public class ReturnStatement : Statement 
    {
        public Expression expression;

        public ReturnStatement(Expression _expression)
        {
            expression = _expression;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public abstract class Expression : Node
    {
        public virtual T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public class Constant : Expression 
    {
        public string integerLiteral;

        public Constant(string _integerLiteral)
        {
            integerLiteral = _integerLiteral;
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}