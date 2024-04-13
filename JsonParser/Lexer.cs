namespace JsonParser;

internal sealed class Lexer
{
    private readonly string _json;

    private static readonly Dictionary<char, char> _mapping = new()
    {
        ['{'] = '}'
    };

    public Lexer(string json)
    {
        _json = json;
    }

    public bool IsValid()
    {
        var stack = new Stack<char>();

        for (int i = 0; i < _json.Length; i++)
        {
            var ch = _json[i];

            if (stack.TryPop(out var expecting) && expecting.Equals(ch))
            {
                continue;
            }

            if (!_mapping.TryGetValue(ch, out var closure))
            {
                return false;
            }

            stack.Push(closure);
        }

        return stack.Count == 0;
    }
}
