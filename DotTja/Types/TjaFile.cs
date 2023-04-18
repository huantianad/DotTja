namespace DotTja.Types;

using System.Collections.Immutable;

public sealed record TjaFile(
    Metadata Metadata,
    ImmutableList<Course> Courses
);
