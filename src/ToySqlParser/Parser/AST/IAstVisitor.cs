namespace ToySqlParser.Parser.AST;

public interface IAstVisitor<out T>
{
    T Visit(SelectStatement statement);
    T Visit(BinaryExpression expression);
    T Visit(IdentifierExpression expression);
    T Visit(LiteralExpression expression);
}
