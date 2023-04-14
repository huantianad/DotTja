namespace DotTja.Types;

/// <summary>
/// Represents the level file's metadata, attempting to a one-to-one representation.
/// Doesn't include any validation, nor default value handling, as this is meant
/// to represent missing fields in the file itself. Thus, most values are optional,
/// as they are optional as defined in the spec. The exception to this is
/// <see cref="Title"/> and <see cref="Subtitle"/> as those seem to be mandatory parts of metadata.
/// </summary>
public record Metadata(
    LocalizedString Title,
    LocalizedString Subtitle
)
{
    public double? Bpm { get; init; }
    public FileInfo? Wave { get; init; }
    public double? Offset { get; init; }
    public double? DemoStart { get; init; }
    public string? Genre { get; init; }
    public ScoreMode ScoreMode { get; init; }
    public string? Maker { get; init; }
    public FileInfo? Lyrics { get; init; }
    public double? SongVol { get; init; }
    public double? SeVol { get; init; }
    public Side? Side { get; init; }
    public int? Life { get; init; }
    public Game? Game { get; init; }
    public double? HeadScroll { get; init; }
    public FileInfo? BgImage { get; init; }
    public FileInfo? BgMovie { get; init; }
    public double? MovieOffset { get; init; }
    public TaikoWebSkin? TaikoWebSkin { get; init; }
}

public enum ScoreMode
{
    AcGen1To7,
    AcGen8To14,
    AcGen0
}

public enum Side
{
    Normal, // "1"
    Ex, // "2"
    Both // "3"
}

/// <summary>
/// Default is <see cref="Taiko"/>, game will be forced to autoplay mode with <see cref="Jube"/>.
/// </summary>
public enum Game
{
    Taiko,
    Jube
}
