namespace ToySqlParser.Parser.AST.ParseStatementImpl;

using ToySqlParser.Parser.AST.Interfaces;
using ToySqlParser.Lexer;

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
        var columns = ParseColumns(context);
        context.Consume(TokenType.Keyword , "FROM");
        var table = context.Consume(TokenType.Identifier).Value;
        Expression? where = null;
        if(context.Match(TokenType.Keyword , "WHERE")){
            context.Consume(TokenType.Keyword, "WHERE");

            where = context.ParseExpression();
        }
        if(!context.Match(TokenType.EndOfFile)){
            throw new Exception($"Unexpected token: {context.Current.Type} {context.Current.Value}");
        }
        return new SelectStatement(columns, table, where);
    }

    /// <summary>
    /// Parses the columns of a select statement.
    /// </summary>
    /// <param name="context">The parser context.</param>
    /// <returns>The list of columns.</returns>
    private List<string> ParseColumns(Parser context){
        var columns = new List<string>();
        while(true){
            if(context.Match(TokenType.Identifier)){
                columns.Add(context.Consume(TokenType.Identifier).Value);
            }
            else if(context.Match(TokenType.Symbol, "*")){
                columns.Add(context.Consume(TokenType.Symbol, "*").Value);
            }
            else{
                throw new Exception("Expected column name or *");
            }
            if(context.Match(TokenType.Symbol, ",")){
                context.Consume(TokenType.Symbol, ",");
                continue;
            }
            break;
        }
        return columns;
    }
}