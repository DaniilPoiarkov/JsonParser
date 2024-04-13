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
        }

        return result;
    }
}
