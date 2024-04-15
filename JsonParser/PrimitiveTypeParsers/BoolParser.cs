namespace JsonParser.PrimitiveTypeParsers;

internal sealed class BoolParser : IPrimitiveTypeParser
{
    public (int Count, object? Value) Parse(string json, int position)
    {
        var ch = json[position];

        if (ch is 't')
        {
            return (4, true);
        }

        if (ch is 'f')
        {
            return (5, false);
        }

        throw new InvalidOperationException($"Unexpected character {ch} during boolean value parsing in position {position}");
    }
}
