namespace JsonParser.Constants;

internal static class TokenTypes
{
    public static readonly TokenType LPar = new("LPar", "{");

    public static readonly TokenType RPar = new("RPar", "}");

    public static readonly TokenType Space = new("Space", " ");

    public static readonly IReadOnlyList<TokenType> List =
        [
            LPar,
            RPar,
            Space
        ];
}
