namespace JsonParser.ValueParsers;

internal sealed class NullParser : IValueParser
{
    public (int Count, object? Result) Parse(string json, int position)
    {
        return (4, null);
    }
}
