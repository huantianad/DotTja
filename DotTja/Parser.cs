namespace DotTja;

using Exceptions;
using Types;
using Types.Builders;

internal static class Parser
{
    private static string ReadLineOrThrow(this TextReader reader) =>
        reader.ReadLine() ?? throw new ParsingException("Encountered end of stream early!");

    public static TjaFile Deserialize(TextReader reader)
    {
        var result = new TjaFileBuilder();

        string line;

        // Parse the
        while (true)
        {
            line = reader.ReadLineOrThrow().Trim();

            if (line == "" || line.StartsWith("//", StringComparison.InvariantCulture))
            {
                continue;
            }

            var split = line.Split(':', 2);

            if (split.Length != 2)
            {
                throw new ParsingException($"Line missing colon separator: {line}");
            }

            var (key, value) = (split[0].Trim(), split[1].Trim());

            if (key == "COURSE")
            {
                break;
            }

            result.Metadata.SetValue(key, value);
        }


        return result.ToTjaFile();
    }
}
