using System.Text.Json.Serialization;

namespace ToySqlParser.Parser.AST;

[JsonDerivedType(typeof(WildcardColumn),    "wildcard")]
[JsonDerivedType(typeof(ExpressionColumn),  "identifier")]
[JsonDerivedType(typeof(AliasedColumn),     "alias")]
public abstract record SelectColumn;

/// <summary>Represents the bare <c>*</c> wildcard in a SELECT list.</summary>
public record WildcardColumn : SelectColumn;

/// <summary>Represents a single expression in the SELECT list with no alias, e.g. <c>id</c>.</summary>
public record ExpressionColumn(Expression Expr) : SelectColumn;

/// <summary>Represents an aliased expression in the SELECT list, e.g. <c>price * 1.1 AS new_price</c>.</summary>
public record AliasedColumn(Expression Expr, string Alias) : SelectColumn;
