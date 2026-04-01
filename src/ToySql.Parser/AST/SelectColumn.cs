using System.Text.Json.Serialization;
using ToySql.Parser.AST.Interfaces;

namespace ToySql.Parser.AST;

[JsonDerivedType(typeof(WildcardColumn), "wildcard")]
[JsonDerivedType(typeof(ExpressionColumn), "identifier")]
[JsonDerivedType(typeof(AliasedColumn), "alias")]
public abstract record SelectColumn : Node;

/// <summary>Represents the bare <c>*</c> wildcard in a SELECT list.</summary>
public record WildcardColumn : SelectColumn
{
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}

/// <summary>Represents a single expression in the SELECT list with no alias, e.g. <c>id</c>.</summary>
public record ExpressionColumn(Expression Expr) : SelectColumn
{
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}

/// <summary>Represents an aliased expression in the SELECT list, e.g. <c>price * 1.1 AS new_price</c>.</summary>
public record AliasedColumn(Expression Expr, string Alias) : SelectColumn
{
    public override T Accept<T>(IAstVisitor<T> visitor) => visitor.Visit(this);
}