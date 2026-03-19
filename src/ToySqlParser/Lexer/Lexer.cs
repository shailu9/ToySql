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
// in the input and provides a method to retrieve the next token. The NextToken method will contain the logic 
// to identify and return the appropriate token based on the current position in the input string.
// </summary>
public class Lexer
{
    // This is the input we get
    private readonly string _input;
    // This is the current position tracker
    private int _position = 0;

    public Lexer(string input)
    {
        _input = input;
    }

    /// <summary>
    /// The NextToken method will contain the logic to identify and return the appropriate token based on the current position in the input string.
    /// </summary>
    /// <returns></returns>
    public Token NextToken()
    {
        while (_position < _input.Length && Char.IsWhiteSpace(_input[_position]))
            _position++;

        if (_position >= _input.Length)
            return new Token(TokenType.EndOfFile, "");

        char current = _input[_position];

        // Handle single quotes
        if(current == '\'')
        {
            _position++; // skip the opening quote
            int start = _position;

            while(_position < _input.Length && _input[_position]!= '\'')
                _position++;
            
            if(_position >= _input.Length)
                throw new Exception("Unterminated string literal");
            
            string value = _input[start.._position];
            _position++; // skip the closing quote

            return new Token(TokenType.StringLiteral,value);
        } 
        // Handle words
        if (char.IsLetter(current))
        {
            int start = _position;
            while (_position < _input.Length && Char.IsLetterOrDigit(_input[_position]))
                _position++;

            string word = _input[start.._position];
            var upper = word.ToUpper();
            if (upper is "SELECT" or "FROM" or "WHERE" or "AND" or "OR")
                return new Token(TokenType.Keyword, upper);

            return new Token(TokenType.Identifier, word);
        }

        // Handle Symbols
        if (current is '*' or ',' or '=')
        {
            _position++;
            return new Token(TokenType.Symbol, current.ToString());
        }

        // Handle Numbers (Literals)
        if (char.IsDigit(current))
        {
            int start = _position;
            while (_position < _input.Length && char.IsDigit(_input[_position]))
                _position++;

            return new Token(TokenType.NumericLiteral, _input[start.._position]);

        }
        throw new Exception($"Unexpected character: {current}");
    }
}