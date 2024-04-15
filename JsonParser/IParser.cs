namespace JsonParser;

internal interface IParser
{
    (int Count, object? Value) Parse(string json, int position);
}
