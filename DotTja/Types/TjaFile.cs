namespace DotTja.Types;

using System.Collections.Immutable;

public sealed partial record TjaFile(
    Metadata Metadata,
    ImmutableList<Course> Courses
);
