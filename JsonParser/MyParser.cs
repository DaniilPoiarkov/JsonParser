namespace JsonParser;

public sealed class MyParser
{
    public static object Parse(string json)
    {
        var lexer = new Lexer(json);

        if (lexer.IsValid())
        {
            
        }

        return new();
    }
}
