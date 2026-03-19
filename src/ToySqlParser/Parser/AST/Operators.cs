namespace ToySqlParser.Parser.AST;

public static class Operators
{
    public static readonly HashSet<string> GetAllOperators = new()
    {
        "=",
        ">",
        "<",
        "!=",
        "AND",
        "OR"
    };
}