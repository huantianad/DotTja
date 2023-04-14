namespace DotTja.Types;

using System.Collections.Immutable;

public record TjaFile(
    Metadata Metadata,
    ImmutableList<Course> Courses
);
