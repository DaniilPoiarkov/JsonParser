using JsonParser.ValueParsers;

namespace JsonParser;

public sealed class Parser
{
    public static object? Parse(string json)
    {
        return InternalParse(json, 0).Value;
    }

    private static (int Count, object? Value) InternalParse(string json, int position)
    {
        for (int i = position; i < json.Length; i++)
        {
            var ch = json[i];

            if (ch == ' ')
            {
                continue;
            }

            return ch is '['
                ? ParseArray(json, i)
                : ParseValue(json, i);
        }

        throw new ArgumentException("Invalid json format.", nameof(json));
    }

    private static (int Count, object?[] Value) ParseArray(string json, int position)
    {
        var result = new List<object?>();

        var i = position + 1;

        for (; i < json.Length; i++)
        {
            var ch = json[i];

            if (ch is ' ' or ',')
            {
                continue;
            }

            if (ch is ']')
            {
                break;
            }

            var (inc, val) = InternalParse(json, i);

            i += inc;
            result.Add(val);
        }

        return (i - position, result.ToArray());
    }

    private static (int Count, object? Value) ParseValue(string json, int position)
    {
        object? result = null;

        var i = position;

        for (; i < json.Length; i++)
        {
            var ch = json[i];

            if (ch == ' ')
            {
                continue;
            }

            var parser = GetValueParser(ch)
                ?? throw new ArgumentException($"Unknown character {ch} on position {i}.", nameof(json));

            return parser.Parse(json, i);
        }

        return (i - position, result);
    }

    private static IValueParser? GetValueParser(char ch)
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
            return new IntParser();
        }

        if (ch is '"')
        {
            return new StringParser();
        }

        throw new ArgumentException($"Unexpected char {ch}");
    }
}
