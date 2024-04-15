using System.Text;

namespace JsonParser.PrimitiveTypeParsers;

internal sealed class NumberParser : IPrimitiveTypeParser
{
    public (int Count, object? Value) Parse(string json, int position)
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

        var result = sb.ToString();

        return (result.Length, decimal.Parse(result));
    }
}
