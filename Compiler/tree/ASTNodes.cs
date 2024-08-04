namespace Tree
{
    public interface IVisitor<T>
    {
        public T Visit(Program program);
        public T Visit(Function function);
        public T Visit(Statement statement);
        public T Visit(ReturnStatement returnStatement);
        public T Visit(Expression expression);
        public T Visit(Constant constant);
    }   

    public abstract class Node
    {
    }

    public class Program : Node
    {
        public Function function;
        public Program(Function _function)
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

    public abstract class Statement : Node
    {
        public virtual T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
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