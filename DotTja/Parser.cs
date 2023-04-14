namespace DotTja;

using System.Collections.Immutable;

using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

using Types;

internal static class Parser
{
    private static readonly Parser<char, string> UntilEndOfLine = Any.Until(EndOfLine).Map(string.Concat);

    private static readonly Parser<char, Unit> SkipUntilEndOfLine = Any.SkipUntil(EndOfLine);

    private static readonly Parser<char, Unit> Comment =
        Try(SkipWhitespaces.Then(String("//"))).Then(SkipUntilEndOfLine);

    private static readonly Parser<char, char> ColonSeparator =
        Char(':').Before(Whitespace.Optional());

    private static Parser<char, (string key, string rawValue)> KeyValuePair(
        Parser<char, string> keyParser
    ) =>
        from _ in Comment.SkipMany()
        from key in keyParser.Before(ColonSeparator)
        from rawValue in UntilEndOfLine
        select (key, rawValue);

    private static readonly Parser<char, Metadata> Metadata =
        KeyValuePair(Uppercase.AtLeastOnceString())
            .Slice()
            .Until(KeyValuePair(String("COURSE")));

    private static readonly Parser<char, ImmutableList<Course>> Courses =
        from _ in Any.Before(End)
        select ImmutableList<Course>.Empty;

    private static readonly Parser<char, TjaFile> File =
        from metadata in Metadata
        from courses in Courses
        select new TjaFile(metadata, courses);

    public static TjaFile Deserialize(IEnumerable<char> input) => File.ParseOrThrow(input);

}
