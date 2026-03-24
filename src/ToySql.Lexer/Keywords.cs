namespace ToySql.Lexer;

public static class Keywords
{
    public static Dictionary<string, TokenType> All = new(StringComparer.OrdinalIgnoreCase)
    {
        { "SELECT", TokenType.Keyword },
        { "FROM",   TokenType.Keyword },
        { "WHERE",  TokenType.Keyword },
        { "AND",    TokenType.Keyword },
        { "OR",     TokenType.Keyword },
        { "AS",     TokenType.Keyword },   // alias keyword
    };
}