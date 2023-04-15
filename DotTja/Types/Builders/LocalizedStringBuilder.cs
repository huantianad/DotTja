namespace DotTja.Types.Builders;

internal class LocalizedStringBuilder
{
    public string? Default { get; set; }
    public string? Ja { get; set; }
    public string? En { get; set; }
    public string? Cn { get; set; }
    public string? Tw { get; set; }
    public string? Ko { get; set; }

    public LocalizedString ToLocalizedString() =>
        new(this.Default ?? throw new InvalidOperationException(
            "Default value was not set before calling ToLocalizedString()."
        ))
        {
            Ja = this.Ja,
            En = this.En,
            Cn = this.Cn,
            Tw = this.Tw,
            Ko = this.Ko
        };
}