using ToySqlParser.Lexer;
using ToySqlParser.Parser.AST;

namespace ToySqlParser.Parser;

// <summary>
// The Parser class is responsible for parsing the input SQL string into an Abstract Syntax Tree (AST).
// It uses the Lexer to tokenize the input and then recursively parses the tokens into an AST.
// </summary>
public partial class Parser
{
    private readonly Lexer.Lexer _lexer;
    private Token _current;

    public Parser(Lexer.Lexer lexer)
    {
        _lexer = lexer;
        _current = _lexer.NextToken();
    }

    // Check if current token matches the expected type and optional value
    // Example: Match(TokenType.Keyword, "SELECT")
    private bool Match(TokenType type, string? value = null)
    {
        if (_current.Type != type)
            return false;
        if (value != null && _current.Value != value)
            return false;
        return true;
    }

    // Consume token and advance to next token
    // Example: Consume(TokenType.Keyword, "SELECT")
    private Token Consume(TokenType type, string? value = null)
    {
        if (!Match(type, value))
            throw new Exception($"Expected {type} {value ?? ""} but got {_current.Type} {_current.Value}");

        var token = _current;
        _current = _lexer.NextToken();
        return token;
    }

    // Parse select statement
    // Example: SELECT id, name FROM users WHERE id = 5 AND name = 'john'
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

    // Parse columns (identifiers or *) separated by commas
    // Example: SELECT id, name, * FROM users
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

}