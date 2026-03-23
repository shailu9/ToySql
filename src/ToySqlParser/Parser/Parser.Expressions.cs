using ToySqlParser.Lexer;
using ToySqlParser.Parser.AST;

namespace ToySqlParser.Parser;

public partial class Parser
{
    // Parse expression (handles operator precedence)
    // Example: id = 5 AND name = 'john'
    public Expression ParseExpression()
    {
        return ParseOrExpression();
    }

    // Parse OR expressions
    // Example: id = 5 OR name = 'john'
    private Expression ParseOrExpression()
    {
        var left = ParseAndExpression();

        while (Match(TokenType.Keyword, "OR"))
        {
            var op = Consume(TokenType.Keyword, "OR").Value;
            var right = ParseAndExpression();
            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    // Parse AND expressions
    // Example: id = 5 AND name = 'john'
    private Expression ParseAndExpression()
    {
        var left = ParseComparisonExpression();

        while (Match(TokenType.Keyword, "AND"))
        {
            var op = Consume(TokenType.Keyword, "AND").Value;
            var right = ParseComparisonExpression();
            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    // Parse comparison expressions
    // Example: id = 5, price > 100
    private Expression ParseComparisonExpression()
    {
        var left = ParseAdditiveExpression();

        if (Match(TokenType.Symbol) && Operators.ComparisonOperators.Contains(_current.Value))
        {
            var op = Consume(TokenType.Symbol).Value;
            var right = ParseAdditiveExpression();
            return new BinaryExpression(left, op, right);
        }

        return left;
    }

    // Parse additive expressions  (+, -)
    // Example: price + 10, total - discount
    private Expression ParseAdditiveExpression()
    {
        var left = ParseMultiplicativeExpression();

        while (Match(TokenType.Symbol) && Operators.AdditiveOperators.Contains(_current.Value))
        {
            var op = Consume(TokenType.Symbol).Value;
            var right = ParseMultiplicativeExpression();
            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    // Parse multiplicative expressions (*, /)
    // Example: price * 1.1, qty / 2
    private Expression ParseMultiplicativeExpression()
    {
        var left = ParsePrimary();

        while (Match(TokenType.Symbol) && Operators.MultiplicativeOperators.Contains(_current.Value))
        {
            var op = Consume(TokenType.Symbol).Value;
            var right = ParsePrimary();
            left = new BinaryExpression(left, op, right);
        }

        return left;
    }

    // Parse primary expressions (identifiers, numeric literals, string literals)
    // Example: id, 5, 1.1, 'john'
    private Expression ParsePrimary()
    {
        if (Match(TokenType.Identifier))
        {
            var name = Consume(TokenType.Identifier).Value;
            return new IdentifierExpression(name);
        }

        if (Match(TokenType.NumericLiteral))
        {
            var value = Consume(TokenType.NumericLiteral).Value;
            return new LiteralExpression(value);
        }

        if (Match(TokenType.StringLiteral))
        {
            var value = Consume(TokenType.StringLiteral).Value;
            return new LiteralExpression(value);
        }
        throw new Exception($"Unexpected token: {_current.Type} {_current.Value}");
    }
}
