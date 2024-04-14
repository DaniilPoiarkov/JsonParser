using System.Text;

namespace JsonParser.ValueParsers;

internal sealed class StringParser : IValueParser
{
    private static readonly Lazy<StringParser> _instance = new(() => new StringParser());
    internal static StringParser Instance => _instance.Value;

    private StringParser() { }

    public (int Count, object? Result) Parse(string json, int position)
    {
        var sb = new StringBuilder();

        var i = position + 1;

        for (; i < json.Length; i++)
        {
            var ch = json[i];

            if (ch is '"')
            {
                i++;
                break;
            }

            if (ch is '\\')
            {
                i++;
                sb.Append(json[i]);
                continue;
            }

            sb.Append(ch);
        }

        var word = sb.ToString();

        return (i - position, word);
    }
}
