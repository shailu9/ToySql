using ToySqlParser.Lexer;
using ToySqlParser.Parser.AST;

namespace ToySqlParser.Parser;
public class Parser
{
    private readonly Lexer.Lexer _lexer;
    private Token _current;

    public Parser(Lexer.Lexer lexer)
    {
        _lexer = lexer;
        _current = _lexer.NextToken();
    }

    private bool Match(TokenType type, string? value = null)
    {
        if (_current.Type != type)
            return false;
        if (value != null && _current.Value != value)
            return false;
        return true;
    }
    private Token Consume(TokenType type, string? value = null)
    {
        if (!Match(type, value))
            throw new Exception($"Expected {type} {value ?? ""} but got {_current.Type} {_current.Value}");

        var token = _current;
        _current = _lexer.NextToken();
        return token;
    }

    public SelectStatement Parse()
    {
        Consume(TokenType.Keyword, "SELECT");

        var columns = ParseColumns();

        Consume(TokenType.Keyword, "FROM");
        var tabletoken = Consume(TokenType.Identifier);
        var table = tabletoken.Value;

        Expression? where = null;
        if (Match(TokenType.Keyword, "WHERE"))
        {
            Consume(TokenType.Keyword, "WHERE");
            where = ParseExpression();
        }

        if (!Match(TokenType.EndOfFile))
            throw new Exception($"Unexpected token: {_current.Type} {_current.Value}");

        return new SelectStatement(columns, table, where);
    }


    private List<string> ParseColumns()
    {
        var columns = new List<string>();

        while (true)
        {
            // Expect identifier or *
            if (Match(TokenType.Identifier) || Match(TokenType.Symbol, "*"))
            {
                columns.Add(_current.Value);
                _current = _lexer.NextToken();
            }
            else
            {
                throw new Exception("Expected column name or *");
            }
            // If next token is comma (",") → continue
            if (Match(TokenType.Symbol, ","))
            {
                Consume(TokenType.Symbol, ",");
                continue;
            }
            // Otherwise stop
            break;
        }

        return columns;
    }
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

    private Expression ParseExpression()
    {
        return ParseOrExpression();
    }

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

    private Expression ParseComparisonExpression()
    {
        var left = ParsePrimary();

        if (Match(TokenType.Symbol) && Operators.GetAllOperators.Contains(_current.Value))
        {
            var op = Consume(TokenType.Symbol).Value;
            var right = ParsePrimary();
            return new BinaryExpression(left, op, right);
        }
        return left;
    }
}