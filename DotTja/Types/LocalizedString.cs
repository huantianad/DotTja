namespace DotTja.Types;

/// <summary>
/// Represents a string value that may have different values depending
/// on the user's localization preferences. The <see cref="Default"/>
/// string is required, but the localized values are not.
/// </summary>
public record LocalizedString
{
    /// <summary>
    /// Default value used if the user's current language does not have a
    /// localized value.
    /// </summary>
    public string? Default { get; init; }

    /// <summary>
    /// Japanese localized value.
    /// </summary>
    public string? Ja { get; init; }

    /// <summary>
    /// English localized value.
    /// </summary>
    public string? En { get; init; }

    /// <summary>
    /// Simplified Chinese localized value.
    /// </summary>
    public string? Cn { get; init; }

    /// <summary>
    /// Traditional Chinese localized value.
    /// </summary>
    public string? Tw { get; init; }

    /// <summary>
    /// Korean localized value.
    /// </summary>
    public string? Ko { get; init; }
}
