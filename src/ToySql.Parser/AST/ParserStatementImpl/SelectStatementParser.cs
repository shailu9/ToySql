using ToySql.Lexer;
using ToySql.Parser.AST.Interfaces;

namespace ToySql.Parser.AST.ParserStatementImpl;

/// <summary>
/// Select statement parser.
/// </summary>
public class SelectStatementParser : IStatementParser
{
    /// <summary>
    /// Parses a select statement.
    /// </summary>
    /// <param name="context">The parser context.</param>
    /// <returns>The parsed select statement.</returns>
    public Node Parse(Parser context)
    {
        context.Consume(TokenType.Keyword, "SELECT");
        // DISTINCT Flag - just a boolean consumed if present
        bool isDistinct = false;
        if(context.Match(TokenType.Keyword, "DISTINCT"))
        {
            context.Consume(TokenType.Keyword, "DISTINCT");
            isDistinct = true;
        }
        var columns = ParseColumns(context);
        context.Consume(TokenType.Keyword, "FROM");
        var table = context.Consume(TokenType.Identifier).Value;
        Expression? where = null;
        if (context.Match(TokenType.Keyword, "WHERE"))
        {
            context.Consume(TokenType.Keyword, "WHERE");

            where = context.ParseExpression();
        }
        // ORDER BY clause - optional
        List<OrderByColumn> OrderBy = null;
        if (context.Match(TokenType.Keyword, "ORDER"))
        {
            context.Consume(TokenType.Keyword, "ORDER");
            context.Consume(TokenType.Keyword, "BY");
            OrderBy = ParseOrderByColumns(context);
        }

        // LIMIT clause - optional
        int? limit = null;
        if (context.Match(TokenType.Keyword, "LIMIT"))
        {
            context.Consume(TokenType.Keyword, "LIMIT");
            limit = int.Parse(context.Consume(TokenType.NumericLiteral).Value);
        }

        // OFFSET clause - optional
        int? offset = null;
        if (context.Match(TokenType.Keyword, "OFFSET"))
        {
            context.Consume(TokenType.Keyword, "OFFSET");
            offset = int.Parse(context.Consume(TokenType.NumericLiteral).Value);
        }

        if (!context.Match(TokenType.EndOfFile))
        {
            throw new Exception($"Unexpected token: {context.Current.Type} {context.Current.Value}");
        }

        return new SelectStatement(columns, table, where!, isDistinct, OrderBy!, limit, offset);
    }

    /// <summary>
    /// Parses the column projection list of a SELECT statement.
    /// Each column is one of: wildcard (*), bare expression, or aliased expression (expr AS alias).
    /// Note: * is only treated as wildcard when it is the sole token in a column position;
    /// within an expression (e.g. price * 1.1) it is handled as multiplication by ParseExpression.
    /// </summary>
    private List<SelectColumn> ParseColumns(Parser context)
    {
        var columns = new List<SelectColumn>();
        while (true)
        {
            SelectColumn column;

            // Wildcard: * must appear alone as the column item (not preceded by an expression)
            if (context.Match(TokenType.Symbol, "*"))
            {
                context.Consume(TokenType.Symbol, "*");
                column = new WildcardColumn();
            }
            else
            {
                // Parse the projection expression (supports arithmetic, identifiers, literals)
                var expr = context.ParseExpression();

                // Optional alias: expr AS alias
                if (context.Match(TokenType.Keyword, "AS"))
                {
                    context.Consume(TokenType.Keyword, "AS");
                    var alias = context.Consume(TokenType.Identifier).Value;
                    column = new AliasedColumn(expr, alias);
                }
                else
                {
                    column = new ExpressionColumn(expr);
                }
            }

            columns.Add(column);

            if (context.Match(TokenType.Symbol, ","))
            {
                context.Consume(TokenType.Symbol, ",");
                continue;
            }
            break;
        }
        return columns;
    }

    /// <summary>
    /// Parses the order by column list of a SELECT statement.
    /// </summary>
    /// <param name="context">The parser context.</param>
    /// <returns>The parsed order by column list.</returns>
    private List<OrderByColumn> ParseOrderByColumns(Parser context){
        var columns = new List<OrderByColumn>();
        while (true)
        {
            var expr = context.ParseExpression();
            bool ascending = true;
            if (context.Match(TokenType.Keyword, "DESC"))
            {
                context.Consume(TokenType.Keyword, "DESC");
                ascending = false;
            }
            else if (context.Match(TokenType.Keyword, "ASC"))
            {
                context.Consume(TokenType.Keyword, "ASC");
            }
            columns.Add(new OrderByColumn(expr, ascending));
            if (context.Match(TokenType.Symbol, ","))
            {
                context.Consume(TokenType.Symbol, ",");
                continue;
            }
            break;
        }
        return columns;
    }
}