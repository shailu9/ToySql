namespace ToySqlParser.Parser.AST;

public static class Operators
{
    /// <summary>Comparison operators used in WHERE predicates.</summary>
    public static readonly HashSet<string> ComparisonOperators = new()
    {
        "=", ">", "<", "!=", ">=", "<="
    };

    /// <summary>Additive arithmetic operators.</summary>
    public static readonly HashSet<string> AdditiveOperators = new() { "+", "-" };

    /// <summary>Multiplicative arithmetic operators.</summary>
    public static readonly HashSet<string> MultiplicativeOperators = new() { "*", "/" };

    /// <summary>All operators (kept for backward-compatibility).</summary>
    public static readonly HashSet<string> GetAllOperators = new(
        ComparisonOperators.Concat(AdditiveOperators).Concat(MultiplicativeOperators)
    );
}