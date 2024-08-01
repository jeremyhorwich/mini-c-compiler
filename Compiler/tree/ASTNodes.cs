namespace Tree
{
    public interface IVisitor
    {
        public T Visit<T>(Node node);
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

        public T Accept<T>(IVisitor visitor)
        {
            return visitor.Visit<T>(this);
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

        public T Accept<T>(IVisitor visitor)
        {
            return visitor.Visit<T>(this);
        }
    }

    public abstract class Statement : Node
    {
        public virtual T Accept<T>(IVisitor visitor)
        {
            return visitor.Visit<T>(this);
        }
    }

    public class ReturnStatement : Statement 
    {
        public Expression expression;

        public ReturnStatement(Expression _expression)
        {
            expression = _expression;
        }

        public override T Accept<T>(IVisitor visitor)
        {
            return visitor.Visit<T>(this);
        }
    }

    public abstract class Expression : Node
    {
        public T Accept<T>(IVisitor visitor)
        {
            return visitor.Visit<T>(this);
        }
    }

    public class Constant : Statement 
    {
        public int integerLiteral;

        public Constant(int _integerLiteral)
        {
            integerLiteral = _integerLiteral;
        }

        public override T Accept<T>(IVisitor visitor)
        {
            return visitor.Visit<T>(this);
        }
    }
}