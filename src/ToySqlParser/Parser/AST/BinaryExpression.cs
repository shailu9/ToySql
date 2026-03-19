namespace ToySqlParser.Parser.AST;

public record BinaryExpression(Expression Left , string Op, Expression Right) : Expression;