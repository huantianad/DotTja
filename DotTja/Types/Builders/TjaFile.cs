namespace DotTja.Types;

using System.Collections.Immutable;

public sealed partial record TjaFile
{
    internal sealed class Builder
    {
        public Metadata.Builder Metadata { get; } = new();
        public ImmutableList<Course>.Builder Courses { get; } = ImmutableList.CreateBuilder<Course>();

        public TjaFile ToTjaFile() => new(
            this.Metadata.ToMetadata(),
            this.Courses.ToImmutableList()
        );
    }
}
