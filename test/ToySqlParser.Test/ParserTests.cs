using System.Text.Json;
using FluentAssertions;
using ToySql.Lexer;
using ToySql.Parser;
using ToySql.Parser.AST;

namespace ToySqlParser.Test;

public class ParserTests
{
    [Fact]
    public void Parser_Should_Parse_Select_With_Where()
    {
        var lexer = new Tokenizer("SELECT name FROM users WHERE id = 5");
        var parser = new Parser(lexer);

        var result = parser.Parse() as SelectStatement;
        result.Should().NotBeNull();

        result.Table.Should().Be("users");
        var col = result.Columns.Should().ContainSingle().Which.Should().BeOfType<ExpressionColumn>().Subject;
        ((IdentifierExpression)col.Expr).Name.Should().Be("name");

        var where = result.Where as BinaryExpression;
        where.Should().NotBeNull();

        ((IdentifierExpression)where.Left).Name.Should().Be("id");
        where.Op.Should().Be("=");
        ((LiteralExpression)where.Right).Value.Should().Be("5");
    }

    [Fact]
    public void Parser_Snapshot_Test()
    {
        var lexer = new Tokenizer("SELECT name FROM users WHERE id = 5");
        var parser = new Parser(lexer);

        var ast = parser.Parse() as SelectStatement;

        var json = JsonSerializer.Serialize(ast, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        // Core structure
        json.Should().Contain(@"""Table"": ""users""");
        json.Should().Contain(@"""Columns"": [");

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
        var lexer = new Tokenizer("SELECT FROM users");
        var parser = new Parser(lexer);

        Action act = () => parser.Parse();

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Parser_Should_Throw_On_Trailing_Tokens()
    {
        var lexer = new Tokenizer("SELECT name FROM users WHERE id = 5 INVALID");
        var parser = new Parser(lexer);

        Action act = () => parser.Parse();

        act.Should().Throw<Exception>().WithMessage("Unexpected token*");
    }

    [Fact]
    public void Parser_Should_Parse_Select_With_And()
    {
        var lexer = new Tokenizer("SELECT id, name FROM users WHERE id = 5 AND name = 'john'");
        var parser = new Parser(lexer);

        var result = parser.Parse() as SelectStatement;
        result.Should().NotBeNull();

        result.Columns.Should().HaveCount(2);
        ((IdentifierExpression)((ExpressionColumn)result.Columns[0]).Expr).Name.Should().Be("id");
        ((IdentifierExpression)((ExpressionColumn)result.Columns[1]).Expr).Name.Should().Be("name");

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
        var lexer = new Tokenizer("SELECT * FROM users WHERE id = 5 = 6");
        var parser = new Parser(lexer);

        Action act = () => parser.Parse();

        act.Should().Throw<Exception>().WithMessage("Unexpected token*");
    }

    // --- Alias tests ---

    [Fact]
    public void Parser_Should_Parse_Single_Alias()
    {
        var lexer = new Tokenizer("SELECT id AS emp_id FROM Employees");
        var parser = new Parser(lexer);

        var result = parser.Parse() as SelectStatement;
        result.Should().NotBeNull();

        var col = result.Columns.Should().ContainSingle().Which.Should().BeOfType<AliasedColumn>().Subject;
        ((IdentifierExpression)col.Expr).Name.Should().Be("id");
        col.Alias.Should().Be("emp_id");
    }

    [Fact]
    public void Parser_Should_Parse_Multiple_Aliases()
    {
        var lexer = new Tokenizer("SELECT id AS emp_id, name AS emp_name FROM Employees");
        var parser = new Parser(lexer);

        var result = parser.Parse() as SelectStatement;
        result.Should().NotBeNull();
        result.Columns.Should().HaveCount(2);

        var c0 = result.Columns[0].Should().BeOfType<AliasedColumn>().Subject;
        ((IdentifierExpression)c0.Expr).Name.Should().Be("id");
        c0.Alias.Should().Be("emp_id");

        var c1 = result.Columns[1].Should().BeOfType<AliasedColumn>().Subject;
        ((IdentifierExpression)c1.Expr).Name.Should().Be("name");
        c1.Alias.Should().Be("emp_name");
    }

    [Fact]
    public void Parser_Should_Parse_Wildcard_Column()
    {
        var lexer = new Tokenizer("SELECT * FROM Employees");
        var parser = new Parser(lexer);

        var result = parser.Parse() as SelectStatement;
        result.Should().NotBeNull();

        result.Columns.Should().ContainSingle().Which.Should().BeOfType<WildcardColumn>();
    }

    [Fact]
    public void Parser_Should_Parse_Mixed_Columns()
    {
        var lexer = new Tokenizer("SELECT id, name AS emp_name FROM Employees");
        var parser = new Parser(lexer);

        var result = parser.Parse() as SelectStatement;
        result.Should().NotBeNull();
        result.Columns.Should().HaveCount(2);

        ((IdentifierExpression)((ExpressionColumn)result.Columns[0]).Expr).Name.Should().Be("id");

        var c1 = result.Columns[1].Should().BeOfType<AliasedColumn>().Subject;
        ((IdentifierExpression)c1.Expr).Name.Should().Be("name");
        c1.Alias.Should().Be("emp_name");
    }

    [Fact]
    public void Parser_Should_Parse_Arithmetic_Alias()
    {
        // SELECT price * 1.1 AS new_price FROM products WHERE id = 5
        var lexer = new Tokenizer("SELECT price * 1.1 AS new_price FROM products WHERE id = 5");
        var parser = new Parser(lexer);

        var result = parser.Parse() as SelectStatement;
        result.Should().NotBeNull();

        var col = result.Columns.Should().ContainSingle().Which.Should().BeOfType<AliasedColumn>().Subject;
        col.Alias.Should().Be("new_price");

        var mul = col.Expr.Should().BeOfType<BinaryExpression>().Subject;
        mul.Op.Should().Be("*");
        ((IdentifierExpression)mul.Left).Name.Should().Be("price");
        ((LiteralExpression)mul.Right).Value.Should().Be("1.1");

        var where = result.Where.Should().BeOfType<BinaryExpression>().Subject;
        ((IdentifierExpression)where.Left).Name.Should().Be("id");
        where.Op.Should().Be("=");
        ((LiteralExpression)where.Right).Value.Should().Be("5");
    }
}
