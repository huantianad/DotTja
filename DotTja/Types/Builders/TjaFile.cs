namespace DotTja.Types;

using System.Collections.Immutable;

public sealed partial record TjaFile
{
    internal sealed class Builder
    {
        public Metadata.Builder Metadata { get; } = new();
        public List<Course.Builder> Courses { get; } = new();

        public TjaFile ToTjaFile() => new(
            this.Metadata.ToMetadata(),
            this.Courses.Select(b => b.ToCourse()).ToImmutableList()
        );
    }
}
