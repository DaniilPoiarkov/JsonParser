using System.Text;

namespace JsonParser;

public sealed class Parser
{
    public static object? Parse(string json)
    {
        object? result = null;

        for (int i = 0; i < json.Length; i++)
        {
            var ch = json[i];

            if (ch == 'n')
            {
                i += 4;
                result = null;
                continue;
            }

            if (ch == 't')
            {
                i += 4;
                result = true;
                continue;
            }

            if (ch == 'f')
            {
                i += 5;
                result = false;
                continue;
            }

            if (ch == ' ')
            {
                continue;
            }

            if (char.IsDigit(ch))
            {
                var tuple = ParseNumber(json, i);

                i += tuple.Count;
                result = tuple.Result;

                continue;
            }

            if (ch is '"')
            {
                var tuple = ParseString(json, i);

                i += tuple.Count;
                result = tuple.Result;
                
                continue;
            }

            throw new ArgumentException($"Unknown character {ch} on position {i}.", nameof(json));
        }

        return result;
    }

    private static (int Count, string Result) ParseString(string json, int position)
    {
        var sb = new StringBuilder();

        var i = position + 1;

        for (; i < json.Length; i++)
        {
            var ch = json[i];

            if (ch is '"')
            {
                i++;
                break;
            }

            if (ch is '\\')
            {
                i++;
                sb.Append(json[i]);
                continue;
            }

            sb.Append(ch);
        }

        var word = sb.ToString();

        return (i - position, word);
    }

    private static (int Count, int Result) ParseNumber(string json, int position)
    {
        var sb = new StringBuilder();

        for (int i = position; i < json.Length; i++)
        {
            var ch = json[i];
            
            if (char.IsDigit(ch))
            {
                sb.Append(ch);
            }
            else
            {
                break;
            }
        }

        var num = sb.ToString();
        
        return (num.Length, int.Parse(num));
    }
}
