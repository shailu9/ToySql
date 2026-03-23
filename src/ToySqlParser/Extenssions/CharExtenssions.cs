namespace ToySqlParser.Extenssions;

public static class CharExtenssions
{
    public static bool IsLetterOrDigitOrUnderscore(this char c) => char.IsLetterOrDigit(c) || c == '_';
}