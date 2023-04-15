namespace DotTja;

using Types;

public static class DotTja
{
    public static TjaFile Deserialize(TextReader reader) =>
        Parser.Deserialize(reader);

    public static TjaFile Deserialize(string input)
    {
        using var reader = new StringReader(input);
        return Parser.Deserialize(reader);
    }
}
