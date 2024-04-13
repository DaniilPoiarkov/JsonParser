using System.Diagnostics;

namespace JsonParser.Constants;

internal static class TokenTypes
{
    public static readonly TokenType LPar = new("LPar", "{");

    public static readonly TokenType RPar = new("RPar", "}");


    public static readonly IReadOnlyList<TokenType> List =
        [
            LPar,
            RPar,
        ];
}

[DebuggerDisplay("{Name}: {Regex}")]
internal class TokenType
{
    public string Name { get; }

    public string Regex { get; }

    public TokenType(string name, string regex)
    {
        Name = name;
        Regex = regex;
    }
}
