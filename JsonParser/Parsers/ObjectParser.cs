using JsonParser.PrimitiveTypeParsers;

namespace JsonParser.Parsers;

internal sealed class ObjectParser : IComplexTypeParser
{
    public (int Count, object? Value) Parse(string json, int position)
    {
        var result = new Dictionary<string, object?>();

        var i = position + 1;

        for (; i < json.Length; i++)
        {
            var ch = json[i];

            if (ch is ' ' or ',' or '\n' or '\r')
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
        var (valuesInc, value) = Parser.InternalParse(json, i);
        i += valuesInc;

        return (i - position, key?.ToString()!, value);
    }
}
