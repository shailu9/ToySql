using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using ToySqlParser.Parser.AST;

namespace ToySqlParser.Test;

public class ParserTests
{
    [Fact]
    public void Parser_Should_Parse_Select_With_Where()
    {
        var lexer = new Lexer.Lexer("SELECT name FROM users WHERE id = 5");
        var parser = new Parser.Parser(lexer);

        var result = parser.Parse() as SelectStatement;
        result.Should().NotBeNull();

        result.Table.Should().Be("users");
        result.Columns.Should().ContainSingle().Which.Should().Be("name");

        var where = result.Where as BinaryExpression;
        where.Should().NotBeNull();

        ((IdentifierExpression)where.Left).Name.Should().Be("id");
        where.Op.Should().Be("=");
        ((LiteralExpression)where.Right).Value.Should().Be("5");
    }

    [Fact]
    public void Parser_Snapshot_Test()
    {
        var lexer = new Lexer.Lexer("SELECT name FROM users WHERE id = 5");
        var parser = new Parser.Parser(lexer);

        var ast = parser.Parse() as SelectStatement;

        var json = JsonSerializer.Serialize(ast, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        // Core structure
        json.Should().Contain(@"""Table"": ""users""");
        json.Should().Contain(@"""Columns"": [");
        json.Should().Contain(@"""name""");

        // WHERE clause structure
        json.Should().Contain(@"""Where"": {");
        json.Should().Contain(@"""$type"": ""binary""");

        // Left side
        json.Should().Contain(@"""Name"": ""id""");

        // Operator
        json.Should().Contain(@"""Op"": ""=""");

        // Right side
        json.Should().Contain(@"""Value"": ""5""");
    }

    [Fact]
    public void Parser_Should_Throw_On_Invalid_Query()
    {
        var lexer = new Lexer.Lexer("SELECT FROM users");
        var parser = new Parser.Parser(lexer);

        Action act = () => parser.Parse();

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Parser_Should_Throw_On_Trailing_Tokens()
    {
        var lexer = new Lexer.Lexer("SELECT name FROM users WHERE id = 5 INVALID");
        var parser = new Parser.Parser(lexer);

        Action act = () => parser.Parse();

        act.Should().Throw<Exception>().WithMessage("Unexpected token*");
    }

    [Fact]
    public void Parser_Should_Parse_Select_With_And()
    {
        var lexer = new Lexer.Lexer("SELECT id, name FROM users WHERE id = 5 AND name = 'john'");
        var parser = new Parser.Parser(lexer);

        var result = parser.Parse() as SelectStatement;
        result.Should().NotBeNull();

        result.Columns.Should().BeEquivalentTo("id", "name");

        var where = result.Where as BinaryExpression;
        where.Should().NotBeNull();
        where.Op.Should().Be("AND");

        var left = where.Left as BinaryExpression;
        left.Should().NotBeNull();
        ((IdentifierExpression)left.Left).Name.Should().Be("id");
        left.Op.Should().Be("=");
        ((LiteralExpression)left.Right).Value.Should().Be("5");

        var right = where.Right as BinaryExpression;
        right.Should().NotBeNull();
        ((IdentifierExpression)right.Left).Name.Should().Be("name");
        right.Op.Should().Be("=");
        ((LiteralExpression)right.Right).Value.Should().Be("john");
    }

    [Fact]
    public void Parser_Should_Throw_On_Chained_Comparison()
    {
        var lexer = new Lexer.Lexer("SELECT * FROM users WHERE id = 5 = 6");
        var parser = new Parser.Parser(lexer);

        Action act = () => parser.Parse();

        act.Should().Throw<Exception>().WithMessage("Unexpected token*");
    }
}