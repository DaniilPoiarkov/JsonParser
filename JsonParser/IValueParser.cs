namespace JsonParser;

internal interface IValueParser
{
    (int Count, object? Result) Parse(string json, int position);
}
