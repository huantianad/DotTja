namespace DotTja.Types;

using Enums;

/// <summary>
/// Represents the level file's metadata, attempting to a one-to-one representation.
/// Doesn't include any validation, nor fill any default values, as this is meant
/// to represent missing fields in the file itself. Thus, most values are optional,
/// as they are optional as defined in the spec. The exception to this is
/// <see cref="Title"/> and <see cref="Subtitle"/> as those seem to be mandatory parts of metadata.
/// </summary>
public sealed partial record Metadata
{
    public LocalizedString Title { get; init; } = new();
    public LocalizedString Subtitle { get; init; } = new();
    public double? Bpm { get; init; }
    public FileInfo? Wave { get; init; }
    public double? Offset { get; init; }
    public double? DemoStart { get; init; }
    public string? Genre { get; init; }
    public ScoreMode? ScoreMode { get; init; }
    public string? Maker { get; init; }
    public FileInfo? Lyrics { get; init; }
    public int? SongVol { get; init; } // maybe double?
    public int? SeVol { get; init; } // maybe double?
    public Side? Side { get; init; }
    public int? Life { get; init; }
    public Game? Game { get; init; }
    public double? HeadScroll { get; init; }
    public FileInfo? BgImage { get; init; }
    public FileInfo? BgMovie { get; init; }
    public double? MovieOffset { get; init; }
    public TaikoWebSkin? TaikoWebSkin { get; init; }
}
