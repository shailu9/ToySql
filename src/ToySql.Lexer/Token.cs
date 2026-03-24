namespace ToySql.Lexer;
public record Token(TokenType Type, string Value,int Line=0,int Column=0);
/// TODO: Implement line and column 