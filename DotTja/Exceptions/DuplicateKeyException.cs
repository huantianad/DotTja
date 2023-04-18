namespace DotTja.Exceptions;

public sealed class DuplicateKeyException : ParsingException
{
    public DuplicateKeyException(string key, object oldVal, object duplicateVal) : base(
        $"Attempted to set key '{key}' to '{duplicateVal}', but it was already set to '{oldVal}'."
    )
    {
    }
}
