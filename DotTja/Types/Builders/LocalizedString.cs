namespace DotTja.Types;

using System.Collections.Immutable;
using System.Reflection;

public sealed partial record LocalizedString
{
    internal sealed class Builder
    {
        private string? Default { get; set; }
        private string? Ja { get; set; }
        private string? En { get; set; }
        private string? Cn { get; set; }
        private string? Tw { get; set; }
        private string? Ko { get; set; }

        public static readonly ImmutableDictionary<string, PropertyInfo> Properties =
            typeof(Builder)
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .ToImmutableDictionary(p => p.Name.ToUpperInvariant(), p => p);

        public LocalizedString ToLocalizedString() => new()
        {
            Default = this.Default,
            Ja = this.Ja,
            En = this.En,
            Cn = this.Cn,
            Tw = this.Tw,
            Ko = this.Ko
        };
    }
}
