namespace ToySql.Parser.AST.Interfaces;

/// <summary>
/// Interface for parsing SQL statements.
/// </summary>
internal interface IStatementParser
{
    /// <summary>
    /// Parses a SQL statement.
    /// </summary>
    /// <param name="context">The parser context.</param>
    /// <returns>The parsed statement.</returns>
    Node Parse(Parser context);
}