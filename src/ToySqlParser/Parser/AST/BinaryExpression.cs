namespace ToySqlParser.Parser.AST;

public record BinaryExpression(Expression Left, string Op, Expression Right) : Expression
{
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}