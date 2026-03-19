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
        var table = _current.Value;

        Expression? where = null;
        if (Match(TokenType.Keyword, "WHERE"))
        {
            Consume(TokenType.Keyword, "WHERE");
            where = ParseExpression();
        }
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
        throw new Exception($"Unexpected token: {_current.Type} {_current.Value}");
    }

    private Expression ParseExpression()
    {
        var left = ParsePrimary();

        if (Match(TokenType.Symbol))
        {
            var op = Consume(TokenType.Symbol).Value;
            var right = ParsePrimary();
            return new BinaryExpression(left, op, right);
        }
        return left;
    }
}