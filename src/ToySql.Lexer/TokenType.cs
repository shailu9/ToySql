namespace ToySql.Lexer;

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