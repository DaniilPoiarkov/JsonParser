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

            if (ch is '[')
            {
                var (inc, arr) = ParseArray(json, i);
                return (i - position + inc,  arr);
            }

            if (ch is '{')
            {
                var (inc, obj) = ParseObject(json, i);
                return (i - position + inc, obj);
            }

            var (vInc, val) = ParseValue(json, i);
            return (i - position + vInc, val);
        }

        throw new ArgumentException("Invalid json format.", nameof(json));
    }

    private static (int Count, Dictionary<string, object?> Value) ParseObject(string json, int position)
    {
        var result = new Dictionary<string, object?>();

        var i = position + 1;

        for (; i < json.Length; i++)
        {
            var ch = json[i];

            if (ch is ' ' or ',' or '\n')
            {
                continue;
            }

            if (ch is '}')
            {
                i++;
                break;
            }

            if (ch is '"')
            {
                var (inc, key, value) = ParseObjectProperty(json, i);

                result.Add(key, value);
                i += inc;
            }
        }

        return (i - position, result);
    }

    private static (int Count, string Key, object? Value) ParseObjectProperty(string json, int position)
    {
        var i = position;

        var (keyInc, key) = StringParser.Instance.Parse(json, i);

        i += keyInc;

        while (json[i] != ':')
        {
            i++;
        }

        i++;
        var (valuesInc, value) = InternalParse(json, i);
        i += valuesInc;

        return (i - position, key?.ToString()!, value);
    }

    private static (int Count, object?[] Value) ParseArray(string json, int position)
    {
        var result = new List<object?>();

        var i = position + 1;

        while (i < json.Length)
        {
            var ch = json[i];

            if (ch is ' ' or ',' or '\n')
            {
                i++;
                continue;
            }

            if (ch is ']')
            {
                i++;
                break;
            }

            var (inc, val) = InternalParse(json, i);

            result.Add(val);
            i += inc;
        }

        return (i - position, result.ToArray());
    }

    private static (int Count, object? Value) ParseValue(string json, int position)
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
            return new NumberParser();
        }

        if (ch is '"')
        {
            return StringParser.Instance;
        }

        return null;
    }
}
