using System.Text.Json.Serialization;
using ToySql.Parser.AST.Interfaces;

namespace ToySql.Parser.AST;

[JsonDerivedType(typeof(SelectStatement), "select")]
[JsonDerivedType(typeof(OrderByColumn), "orderByColumn")]
[JsonDerivedType(typeof(WildcardColumn), "wildcard")]
[JsonDerivedType(typeof(ExpressionColumn), "expressionColumn")]
[JsonDerivedType(typeof(AliasedColumn), "aliasedColumn")]
public abstract record Node
{
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}