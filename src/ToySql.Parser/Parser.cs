using ToySql.Lexer;
using ToySql.Parser.AST;
using ToySql.Parser.AST.Interfaces;
using ToySql.Parser.AST.ParserStatementImpl;

namespace ToySql.Parser;

// <summary>
// The Parser class is responsible for parsing the input SQL string into an Abstract Syntax Tree (AST).
// It uses the Lexer to tokenize the input and then recursively parses the tokens into an AST.
// </summary>
public partial class Parser
{
    private readonly Tokenizer _tokenizer;
    private Token _current;
    private readonly Dictionary<string, IStatementParser> _statementParsers;

    public Token Current => _current;
    public Parser(Tokenizer tokenizer)
    {
        _tokenizer = tokenizer;
        _current = _tokenizer.NextToken();
        _statementParsers = new Dictionary<string, IStatementParser>()
        {
            { "SELECT" , new SelectStatementParser()}
            //add more statement parsers here in future
            // { "INSERT" , new InsertStatementParser()}
        };
    }
    // Check if current token matches the expected type and optional value
    // Example: Match(TokenType.Keyword, "SELECT")
    public bool Match(TokenType type, string? value = null)
    {
        if (_current.Type != type)
            return false;
        if (value != null && _current.Value != value)
            return false;
        return true;
    }

    // Consume token and advance to next token
    // Example: Consume(TokenType.Keyword, "SELECT")
    public Token Consume(TokenType type, string? value = null)
    {
        if (!Match(type, value))
            throw new Exception($"Expected {type} {value ?? ""} but got {_current.Type} {_current.Value}");

        var token = _current;
        _current = _tokenizer.NextToken();
        return token;
    }

    public Node Parse()
    {
        if (_statementParsers.TryGetValue(_current.Value, out var parser))
        {
            return parser.Parse(this);
        }
        throw new Exception($"Unexpected token: {_current.Type} {_current.Value}");
    }
}