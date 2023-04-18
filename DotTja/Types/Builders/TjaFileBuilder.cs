namespace DotTja.Types.Builders;

using System.Collections.Immutable;

internal sealed class TjaFileBuilder
{
    public MetadataBuilder Metadata { get; } = new();
    public ImmutableList<Course>.Builder Courses { get; } = ImmutableList.CreateBuilder<Course>();

    public TjaFile ToTjaFile() => new(this.Metadata.ToMetadata(), this.Courses.ToImmutable());
}
