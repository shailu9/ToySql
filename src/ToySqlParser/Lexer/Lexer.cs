namespace ToySqlParser.Lexer;

// <summary>
// TokenType enum defines the types of tokens that can be recognized by the lexer. 
// These include identifiers, keywords, string literals, numeric literals, operators, punctuation, and an end-of-file token to signify the end of the input.
// </summary>
public enum TokenType
{
    Identifier,
    Keyword,
    Symbol,
    NumericLiteral,
    StringLiteral,
    EndOfFile
}

public record Token(TokenType Type, string Value);

// <summary>
// The Lexer class is responsible for tokenizing the input SQL string. It maintains the current position
// in the input and provides a method to retrieve the next token. 
// </summary>
public class Lexer
{
    private readonly string _input;
    private int _position = 0;

    private static readonly Dictionary<string, TokenType> Keywords = new(StringComparer.OrdinalIgnoreCase)
    {
        { "SELECT", TokenType.Keyword },
        { "FROM", TokenType.Keyword },
        { "WHERE", TokenType.Keyword },
        { "AND", TokenType.Keyword },
        { "OR", TokenType.Keyword }
    };

    public Lexer(string input)
    {
        _input = input;
    }

    private char CurrentChar => _position < _input.Length ? _input[_position] : '\0';
    private bool IsAtEnd => _position >= _input.Length;

    private void Advance() => _position++;

    /// <summary>
    /// Identifies and returns the appropriate token based on the current position in the input string.
    /// </summary>
    public Token NextToken()
    {
        SkipWhitespace();

        if (IsAtEnd)
            return new Token(TokenType.EndOfFile, "");

        if (CurrentChar == '\'')
            return ParseStringLiteral();

        if (char.IsLetter(CurrentChar))
            return ParseWord();

        if (char.IsDigit(CurrentChar))
            return ParseNumericLiteral();

        if (CurrentChar is '*' or ',' or '=')
            return ParseSymbol();

        throw new Exception($"Unexpected character: {CurrentChar}");
    }

    private void SkipWhitespace()
    {
        while (!IsAtEnd && char.IsWhiteSpace(CurrentChar))
            Advance();
    }

    private Token ParseStringLiteral()
    {
        Advance(); // skip opening quote
        int start = _position;

        while (!IsAtEnd && CurrentChar != '\'')
            Advance();

        if (IsAtEnd)
            throw new Exception("Unterminated string literal");

        string value = _input.Substring(start, _position - start);
        Advance(); // skip closing quote

        return new Token(TokenType.StringLiteral, value);
    }

    private Token ParseWord()
    {
        int start = _position;

        while (!IsAtEnd && char.IsLetterOrDigit(CurrentChar))
            Advance();

        string word = _input.Substring(start, _position - start);

        if (Keywords.TryGetValue(word, out var type))
            return new Token(type, word.ToUpper());

        return new Token(TokenType.Identifier, word);
    }

    private Token ParseNumericLiteral()
    {
        int start = _position;

        while (!IsAtEnd && char.IsDigit(CurrentChar))
            Advance();

        return new Token(TokenType.NumericLiteral, _input.Substring(start, _position - start));
    }

    private Token ParseSymbol()
    {
        char symbol = CurrentChar;
        Advance();
        return new Token(TokenType.Symbol, symbol.ToString());
    }
}