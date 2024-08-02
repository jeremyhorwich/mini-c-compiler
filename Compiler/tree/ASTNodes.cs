namespace Tree
{
    public interface IVisitor<TReturn, TNode> where TNode : Node
    {
        public TReturn Visit(TNode node);
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

        public T Accept<T>(IVisitor<T, Program> visitor)
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

        public T Accept<T>(IVisitor<T, Function> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public abstract class Statement : Node
    {
        public virtual T Accept<T>(IVisitor<T, Statement> visitor)
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

        public T Accept<T>(IVisitor<T, ReturnStatement> visitor)
        {
            return visitor.Visit(this);
        }
    }

    public abstract class Expression : Node
    {
        public virtual T Accept<T>(IVisitor<T, Expression> visitor)
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

        public T Accept<T>(IVisitor<T, Constant> visitor)
        {
            return visitor.Visit(this);
        }
    }
}