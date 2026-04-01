namespace ToySql.Parser.AST.Interfaces;

public interface IAstVisitor<out T>
{
    T Visit(SelectStatement statement);
    T Visit(BinaryExpression expression);
    T Visit(IdentifierExpression expression);
    T Visit(LiteralExpression expression);
    T Visit(OrderByColumn orderByColumn);
    T Visit(WildcardColumn wildcardColumn);
    T Visit(ExpressionColumn expressionColumn);
    T Visit(AliasedColumn aliasedColumn);
}
