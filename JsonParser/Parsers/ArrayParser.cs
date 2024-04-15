namespace JsonParser.Parsers;

internal sealed class ArrayParser : IComplexTypeParser
{
    public (int Count, object? Value) Parse(string json, int position)
    {
        var result = new List<object?>();

        var i = position + 1;

        while (i < json.Length)
        {
            var ch = json[i];

            if (ch is ' ' or ',' or '\n' or '\r')
            {
                i++;
                continue;
            }

            if (ch is ']')
            {
                i++;
                break;
            }

            var (inc, val) = Parser.InternalParse(json, i);

            result.Add(val);
            i += inc;
        }

        return (i - position, result.ToArray());
    }
}
