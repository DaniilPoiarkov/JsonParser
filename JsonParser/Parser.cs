using JsonParser.Parsers;

namespace JsonParser;

public sealed class Parser
{
    public static object? Parse(string json)
    {
        return InternalParse(json, 0).Value;
    }

    internal static (int Count, object? Value) InternalParse(string json, int position)
    {
        for (int i = position; i < json.Length; i++)
        {
            var ch = json[i];

            if (ch == ' ')
            {
                continue;
            }

            var parser = GetParser(ch);

            var (inc, val) = parser.Parse(json, i);

            return (i - position + inc, val);
        }

        throw new ArgumentException("Invalid json format.", nameof(json));
    }

    private static IComplexTypeParser GetParser(char ch)
    {
        return ch switch
        {
            '[' => new ArrayParser(),
            '{' => new ObjectParser(),
            _ => new ValueParser(),
        };
    }
}
