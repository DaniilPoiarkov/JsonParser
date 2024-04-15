﻿using System.Text;

namespace JsonParser.ValueParsers;

internal sealed class NumberParser : IValueParser
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

        var result = sb.ToString();

        return (result.Length, decimal.Parse(result));
    }
}