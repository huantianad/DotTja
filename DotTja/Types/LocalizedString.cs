namespace DotTja.Types;

/// <summary>
/// Represents a string value that may have different values depending
/// on the user's localization preferences.
/// </summary>
/// <param name="Main">
/// Default value used if translations are not preferred by the user,
/// or if the preferred translation is not set.
/// </param>
/// <param name="Ja">Japanese localized value.</param>
/// <param name="En">English localized value.</param>
/// <param name="Cn">Simplified Chinese localized value.</param>
/// <param name="Tw">Traditional Chinese localized value.</param>
/// <param name="Ko">Korean localized value.</param>
public record LocalizedString(
    string Main,
    string? Ja,
    string? En,
    string? Cn,
    string? Tw,
    string? Ko
);
