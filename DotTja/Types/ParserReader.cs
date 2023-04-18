namespace DotTja.Types;

using Exceptions;

/// <summary>
/// Wrapper around TextReader that buffers lines, keeps track of line number.
/// Used for parsing convenience
///
/// Before reading anything, currentLine == null, LineNumber == 0
///
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
    /// Reads a line from <see cref="textReader"/> while also storing it in <see cref="CurrentLine"/>
    /// and incrementing <see cref="LineNumber"/>.
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
    /// or null if all characters have been read.</returns>
    private string? ReadContentLine()
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
                return line;
            }
        }
    }

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
}
