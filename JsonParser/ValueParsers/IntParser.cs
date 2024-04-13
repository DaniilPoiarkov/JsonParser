using System.Text;

namespace JsonParser.ValueParsers;

internal sealed class IntParser : IValueParser
{
    public (int Count, object? Result) Parse(string json, int position)
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
