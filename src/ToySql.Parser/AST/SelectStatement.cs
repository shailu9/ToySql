using ToySql.Parser.AST.Interfaces;

namespace ToySql.Parser.AST;

public record SelectStatement(List<SelectColumn> Columns,string Table, Expression Where) : Node
{
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}