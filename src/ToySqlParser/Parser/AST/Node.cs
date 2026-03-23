using System.Text.Json.Serialization;
namespace ToySqlParser.Parser.AST;

[JsonDerivedType(typeof(SelectStatement), "select")]
public abstract record Node
{
    public abstract T Accept<T>(IAstVisitor<T> visitor);
}