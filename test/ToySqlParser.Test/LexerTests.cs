using FluentAssertions;
using ToySqlParser.Lexer;

namespace ToySqlParser.Test;

public class LexerTests
{
    [Fact]
    public void Lexer_Should_Tokenize_Select_Query()
    {
        var lexer = new Lexer.Lexer("SELECT name FROM users");

        var tokens = new List<Token>();
        Token token;

        do
        {
            token = lexer.NextToken();
            tokens.Add(token);
        } while (token.Type != TokenType.EndOfFile);

        // Assert
        tokens.Should().BeEquivalentTo(new[]
        {
            new Token(TokenType.Keyword, "SELECT"),
            new Token(TokenType.Identifier, "name"),
            new Token(TokenType.Keyword, "FROM"),
            new Token(TokenType.Identifier, "users"),
            new Token(TokenType.EndOfFile, "")
        });
    }

    [Fact]
    public void Lexer_Should_Tokenize_And_Or()
    {
        var lexer = new Lexer.Lexer("AND OR");
        var tokens = new List<Token>();
        Token token;
        do
        {
            token = lexer.NextToken();
            tokens.Add(token);
        } while (token.Type != TokenType.EndOfFile);

        tokens.Should().BeEquivalentTo(new[]
        {
            new Token(TokenType.Keyword, "AND"),
            new Token(TokenType.Keyword, "OR"),
            new Token(TokenType.EndOfFile, "")
        });
    }
}