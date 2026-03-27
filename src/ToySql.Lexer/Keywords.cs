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
        { "DISTINCT" , TokenType.Keyword},
        { "ORDER", TokenType.Keyword},
        { "BY" , TokenType.Keyword},
        { "ASC" ,TokenType.Keyword},
        { "DESC" , TokenType.Keyword},
        { "LIMIT", TokenType.Keyword},
        { "OFFSET",TokenType.Keyword}
    };
}