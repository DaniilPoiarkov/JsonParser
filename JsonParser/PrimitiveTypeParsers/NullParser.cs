namespace JsonParser.PrimitiveTypeParsers;

internal sealed class NullParser : IPrimitiveTypeParser
{
    public (int Count, object? Value) Parse(string json, int position)
    {
        return (4, null);
    }
}
