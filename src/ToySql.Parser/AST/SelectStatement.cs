using ToySql.Parser.AST.Interfaces;

namespace ToySql.Parser.AST;

public record SelectStatement(
    List<SelectColumn> Columns,
    string Table, 
    Expression Where,
    bool IsDistinct = false,
    List<OrderByColumn> OrderBy = null,
    int? Limit = null,
    int? Offset = null) : Node
{
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}