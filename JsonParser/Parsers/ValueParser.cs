using JsonParser.PrimitiveTypeParsers;

namespace JsonParser.Parsers;

internal sealed class ValueParser : IComplexTypeParser
{
    public (int Count, object? Value) Parse(string json, int position)
    {
        var i = position;

        if (json[i] is ' ')
        {
            i++;
            throw new ArgumentException($"Unexpected character {json[i]} on position {i}.", nameof(json));
        }

        var parser = GetValueParser(json[i])
            ?? throw new ArgumentException($"Unknown character \'{json[i]}\' on position {i}.", nameof(json));

        var (inc, val) = parser.Parse(json, i);

        return (i - position + inc, val);
    }

    private static IPrimitiveTypeParser? GetValueParser(char ch)
    {
        if (ch == 'n')
        {
            return new NullParser();
        }

        if (ch is 't' or 'f')
        {
            return new BoolParser();
        }

        if (char.IsDigit(ch))
        {
            return new NumberParser();
        }

        if (ch is '"')
        {
            return StringParser.Instance;
        }

        return null;
    }
}
