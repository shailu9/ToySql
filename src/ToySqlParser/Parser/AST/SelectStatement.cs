namespace ToySqlParser.Parser.AST;

public record SelectStatement(List<string> Columns,string Table,Expression? Where) : Node;