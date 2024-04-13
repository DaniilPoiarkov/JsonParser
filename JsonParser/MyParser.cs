namespace JsonParser;

public sealed class MyParser
{
    public static void Parse(string json)
    {
        var lexer = new Lexer(json);

        _ = lexer.IsValid();
    }
}
