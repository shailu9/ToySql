using ToySql.Parser.AST.Interfaces;

namespace ToySql.Parser.AST;

public record OrderByColumn(Expression Expression, bool Ascending) : Node
{
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}