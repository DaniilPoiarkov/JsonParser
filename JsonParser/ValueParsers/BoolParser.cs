namespace JsonParser.ValueParsers;

internal sealed class BoolParser : IValueParser
{
    public (int Count, object? Result) Parse(string json, int position)
    {
        var ch = json[position];

        if (ch == 't')
        {
            return (4, true);
        }

        if (ch == 'f')
        {
            return (5, false);
        }

        throw new InvalidOperationException($"Unexpected character {ch} during boolean value parsing in position {position}");
    }
}
