using JsonParser.Constants;

namespace JsonParser;

internal sealed class Token
{
    public TokenType Type { get; }

    public int Position { get; }

    public string Value { get; }

    public Token(TokenType type, int position, string value)
    {
        Type = type;
        Position = position;
        Value = value;
    }
}
