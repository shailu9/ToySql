namespace ToySql.Lexer;

// <summary>
// The Tokenizer class is responsible for tokenizing the input SQL string. It maintains the current position
// in the input and provides a method to retrieve the next token. 
// </summary>
public class Tokenizer
{
    private readonly string _input;
    private int _position = 0;

    private static readonly Dictionary<string, TokenType> ALLKEYWORDS = Keywords.All;

    private char CurrentChar => _position < _input.Length ? _input[_position] : '\0';
    private char PeekNext => _position + 1 < _input.Length ? _input[_position + 1] : '\0';
    private bool IsAtEnd => _position >= _input.Length;

    public Tokenizer(string input)
    {
        _input = input;
    }

    #region Public Member Methods
    /// <summary>
    /// Identifies and returns the appropriate token based on the current position in the input string.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Token NextToken()
    {
        SkipWhiteSpace();

        if (IsAtEnd)
            return new Token(TokenType.EndOfFile, "");

        if (CurrentChar == '\'')
            return ParseStringLiteral();

        if (char.IsLetter(CurrentChar))
            return ParseWord();

        if (char.IsDigit(CurrentChar))
            return ParseNumericLiteral();

        // Two-character symbol: !=, >=, <=
        if (CurrentChar == '!' && PeekNext == '=')
            return ParseSymbolN(2);
        if (CurrentChar == '>' && PeekNext == '=')
            return ParseSymbolN(2);
        if (CurrentChar == '<' && PeekNext == '=')
            return ParseSymbolN(2);

        if (CurrentChar is '*' or ',' or '=' or '+' or '-' or '/' or '>' or '<')
            return ParseSymbol();

        throw new Exception($"Unexpected character: {CurrentChar}");
    }
    #endregion
    #region Private Member Methods
    private void Advance() => _position++;
    private void SkipWhiteSpace()
    {
        while (!IsAtEnd && char.IsWhiteSpace(CurrentChar))
            Advance();
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

        while (!IsAtEnd && CurrentChar.IsLetterOrDigitOrUnderscore())
            Advance();

        string word = _input.Substring(start, _position - start);

        if (ALLKEYWORDS.TryGetValue(word, out var type))
            return new Token(type, word.ToUpper());

        return new Token(TokenType.Identifier, word);
    }

    private Token ParseNumericLiteral()
    {
        int start = _position;

        while (!IsAtEnd && char.IsDigit(CurrentChar))
            Advance();

        // Consume optional decimal fraction (e.g. 1.1, 3.14)
        if (!IsAtEnd && CurrentChar == '.' && char.IsDigit(PeekNext))
        {
            Advance(); // consume '.'
            while (!IsAtEnd && char.IsDigit(CurrentChar))
                Advance();
        }

        return new Token(TokenType.NumericLiteral, _input.Substring(start, _position - start));
    }

    private Token ParseSymbol()
    {
        char symbol = CurrentChar;
        Advance();
        return new Token(TokenType.Symbol, symbol.ToString());
    }

    private Token ParseSymbolN(int length)
    {
        string symbol = _input.Substring(_position, length);
        _position += length;
        return new Token(TokenType.Symbol, symbol);
    }
    #endregion
}