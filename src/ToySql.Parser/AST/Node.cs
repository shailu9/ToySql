using System.Text.Json.Serialization;
using ToySql.Parser.AST.Interfaces;

namespace ToySql.Parser.AST;

[JsonDerivedType(typeof(SelectStatement), "select")]
public abstract record Node
{
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}