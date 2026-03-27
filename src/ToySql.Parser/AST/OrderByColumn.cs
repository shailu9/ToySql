using ToySql.Parser.AST.Interfaces;

namespace ToySql.Parser.AST;

public record OrderByColumn(Expression Expression, bool Ascending);