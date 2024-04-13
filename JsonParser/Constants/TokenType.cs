using System.Diagnostics;
using System.Text.RegularExpressions;

namespace JsonParser.Constants;

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
