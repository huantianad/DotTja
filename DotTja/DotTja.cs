namespace DotTja;

using Exceptions;
using Types;

public static class DotTja
{
    public static TjaFile Deserialize(TextReader reader)
    {
        var parserReader = new ParserReader(reader);
        try
        {
            return Parser.Deserialize(parserReader);
        }
        catch (Exception e)
        {
            var message =
                $"Encountered error while parsing at LineNumber = {parserReader.LineNumber}, " +
                $"CurrentLine = '{parserReader.CurrentLine}'.";
            throw new ParsingException(message, e);
        }
    }

    public static TjaFile Deserialize(string input)
    {
        using var reader = new StringReader(input);
        return Deserialize(reader);
    }
}
