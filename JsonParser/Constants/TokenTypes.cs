using System.Diagnostics;
using System.Text.RegularExpressions;

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

[DebuggerDisplay("{Name}: {Regex}")]
internal class TokenType
{
    public string Name { get; }

    public string RegexPattern { get; }

    public Regex Regex { get; }

    public TokenType(string name, string regex)
    {
        Name = name;
        RegexPattern = regex;
        Regex = new Regex($@"^{regex}");
    }
}
