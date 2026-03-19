using System.Text.Json.Serialization;
namespace ToySqlParser.Parser.AST;

[JsonDerivedType(typeof(BinaryExpression), "binary")]
[JsonDerivedType(typeof(IdentifierExpression), "identifier")]
[JsonDerivedType(typeof(LiteralExpression), "literal")]
public record Expression : Node;