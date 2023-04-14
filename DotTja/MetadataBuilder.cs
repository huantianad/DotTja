namespace DotTja;

using Types;

public class MetadataBuilder
{
    private LocalizedString Title { get; set; }
    private LocalizedString Subtitle { get; set; }
    private double? Bpm { get; set; }
    private FileInfo? Wave { get; set; }
    private double? Offset { get; set; }
    private double? DemoStart { get; set; }
    private string? Genre { get; set; }
    private ScoreMode ScoreMode { get; set; }
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

    public Metadata ToMetadata() => new(
        this.Title,
        this.Subtitle
    )
    {
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

    public void SetValue(string key, string rawValue)
    {
    }
}
