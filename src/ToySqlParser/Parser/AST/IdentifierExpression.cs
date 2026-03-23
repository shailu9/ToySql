namespace ToySqlParser.Parser.AST;

public record IdentifierExpression(string Name) : Expression
{
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}