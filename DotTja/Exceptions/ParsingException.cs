namespace DotTja.Exceptions;

public class ParsingException : Exception
{
    public ParsingException(string message, Exception? inner = null) : base(message, inner)
    {
    }
}
