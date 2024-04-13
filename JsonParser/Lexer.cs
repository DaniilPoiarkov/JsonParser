using JsonParser.Constants;

namespace JsonParser;

internal sealed class Lexer
{
    private readonly string _json;

    private int Position;

    private readonly List<Token> _tokens = [];

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

            if (ch.Equals(' '))
            {
                continue;
            }
            
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

    public IEnumerable<Token> Analyse()
    {
        while (MoveNext())
        {

        }

        return _tokens.Where(t => t.Type.Name != TokenTypes.Space.Name);
    }

    private bool MoveNext()
    {
        if (Position >= _json.Length)
        {
            return false;
        }

        var tokens = TokenTypes.List;

        var substring = _json[Position..];

        for (int i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            var match = token.Regex.Match(substring);

            if (match.Success && !string.IsNullOrEmpty(match.Value))
            {
                Position += match.Value.Length;

                _tokens.Add(
                    new Token(token, Position, substring)
                );

                return true;
            }
        }

        throw new ArgumentException($"Input json is invalid! Unexpected character in position {Position}.", "json");
    }
}
