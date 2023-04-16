namespace DotTja.Types.Builders;

using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Enums;
using Exceptions;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
internal class MetadataBuilder
{
    public LocalizedStringBuilder Title { get; } = new();
    public LocalizedStringBuilder Subtitle { get; } = new();
    private double? Bpm { get; set; }
    private FileInfo? Wave { get; set; }
    private double? Offset { get; set; }
    private double? DemoStart { get; set; }
    private string? Genre { get; set; }
    private ScoreMode? ScoreMode { get; set; }
    private string? Maker { get; set; }
    private FileInfo? Lyrics { get; set; }
    private double? SongVol { get; set; }
    private double? SeVol { get; set; }
    private Side? Side { get; set; }
    private int? Life { get; set; }
    private Game? Game { get; set; }
    private double? HeadScroll { get; set; }
    private FileInfo? BgImage { get; set; }
    private FileInfo? BgMovie { get; set; }
    private double? MovieOffset { get; set; }
    private TaikoWebSkin? TaikoWebSkin { get; set; }

    public Metadata ToMetadata() => new()
    {
        Title = this.Title.ToLocalizedString(),
        Subtitle = this.Subtitle.ToLocalizedString(),
        Bpm = this.Bpm,
        Wave = this.Wave,
        Offset = this.Offset,
        DemoStart = this.DemoStart,
        Genre = this.Genre,
        ScoreMode = this.ScoreMode,
        Maker = this.Maker,
        Lyrics = this.Lyrics,
        SongVol = this.SongVol,
        SeVol = this.SeVol,
        Side = this.Side,
        Life = this.Life,
        Game = this.Game,
        HeadScroll = this.HeadScroll,
        BgImage = this.BgImage,
        BgMovie = this.BgMovie,
        MovieOffset = this.MovieOffset,
        TaikoWebSkin = this.TaikoWebSkin
    };

    public static readonly ImmutableDictionary<string, PropertyInfo> Properties =
        typeof(MetadataBuilder)
            .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .ToImmutableDictionary(p => p.Name.ToUpperInvariant(), p => p);
}
