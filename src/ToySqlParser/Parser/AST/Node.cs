namespace ToySqlParser.Parser.AST;

public abstract record Node
{
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}