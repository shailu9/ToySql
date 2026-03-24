using System.Text.Json.Serialization;

namespace ToySql.Parser.AST;

[JsonDerivedType(typeof(BinaryExpression), "binary")]
[JsonDerivedType(typeof(IdentifierExpression), "identifier")]
[JsonDerivedType(typeof(LiteralExpression), "literal")]
public abstract record Expression : Node;