namespace DotTja.Types;

using Exceptions;

/// <summary>
/// Wrapper around TextReader that buffers lines, keeps track of line number.
/// Used for parsing convenience
///
/// Before reading anything, currentLine == null, LineNumber == 0
/// After reading something, currentLine and LineNumber will be set to values of the last line read.
/// </summary>
public sealed class ParserReader
{
    private readonly TextReader textReader;
    private bool reusingLastLine;

    public string? CurrentLine { get; private set; }
    public int LineNumber { get; private set; }

    public ParserReader(TextReader textReader) => this.textReader = textReader;

    /// <summary>
    /// Checks if the given one-line string is a comment or not.
    /// </summary>
    /// <param name="line">A singe line from the stream.</param>
    /// <returns>Whether or not the given line is a comment.</returns>
    private static bool IsComment(string line) =>
        line.TrimStart().StartsWith("//", StringComparison.InvariantCulture);

    /// <summary>
    /// Tells the <see cref="ParserReader"/> to reuse the current line instead of
    /// reading a new line the next time <see cref="ReadLine"/> is called.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if called twice in a row without a <see cref="ReadLine"/> in between.
    /// </exception>
    public void ReuseLastLine()
    {
        if (this.reusingLastLine)
        {
            throw new InvalidOperationException(
                $"Called {nameof(this.ReuseLastLine)} while already set to reuse last line."
            );
        }
        this.reusingLastLine = true;
    }

    /// <summary>
    /// <para>
    ///     Reads a line from <see cref="textReader"/> while also storing it in <see cref="CurrentLine"/>
    ///     and incrementing <see cref="LineNumber"/>.
    /// </para>
    /// <para>
    ///     If <see cref="reusingLastLine"/> is set to true, then instead of reading a new line
    ///     from the buffer, this will instead simply return the <see cref="CurrentLine"/>,
    ///     and toggle <see cref="reusingLastLine"/> to false.
    /// </para>
    /// </summary>
    /// <returns>The next line from <see cref="textReader"/>, or null if all characters have been read.</returns>
    private string? ReadLine()
    {
        if (this.reusingLastLine)
        {
            this.reusingLastLine = false;
            return this.CurrentLine;
        }

        this.CurrentLine = this.textReader.ReadLine();
        this.LineNumber++;
        return this.CurrentLine;
    }

    /// <summary>
    /// Reads lines from <see cref="textReader"/> until we get one that isn't empty or isn't a comment.
    /// Returns null if it reaches the end of the stream while doing so.
    /// </summary>
    /// <returns>The next non-empty and non-comment line in <see cref="textReader"/>,
    /// or null if all characters have been read, or if there are only empty/comment lines until end of file.</returns>
    public string? ReadContentLine()
    {
        while (true)
        {
            var line = this.ReadLine();

            if (line == null)
            {
                return line;
            }

            if (!string.IsNullOrWhiteSpace(line) && !IsComment(line))
            {
                return line.Trim();
            }
        }
    }

    /// <summary>
    /// Same as <see cref="ReadContentLine"/> but doesn't advance the position in the stream.
    /// Returns null if there are no more non-empty/non-comment lines until end of file.
    /// </summary>
    public string? PeekContentLine()
    {
        var line = this.ReadContentLine();
        this.ReuseLastLine();
        return line;
    }

    /// <summary>
    /// Uses same logic as <see cref="ReadContentLine"/> to read the next line, then splits
    /// it into a key value pair split by a ':'. Will return null if there are no more content
    /// lines until the end of the file.
    /// </summary>
    /// <returns>Tuple of key and value, each trimmed. Null if no more lines.</returns>
    /// <exception cref="ParsingException">Raised if next content line doesn't have a colon.</exception>
    public (string key, string value)? ReadKeyValuePair()
    {
        var line = this.ReadContentLine();
        if (line == null)
        {
            return null;
        }

        var split = line.Split(':', 2);
        if (split.Length != 2)
        {
            throw new ParsingException("Expected key-value pair, but line is missing a colon separator.");
        }

        return (split[0].Trim(), split[1].Trim());
    }

    /// <summary>
    /// Same ase <see cref="ReadKeyValuePair()"/>, except it raises a <see cref="ParsingException"/>
    /// instead of returning null.
    /// </summary>
    /// <param name="message">Message for the exception.</param>
    /// <param name="exception">Optional inner exception.</param>
    public (string key, string value) ReadKeyValuePair(string message, Exception? exception = null) =>
        this.ReadKeyValuePair() ?? throw new ParsingException(message, exception);
}
