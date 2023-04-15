namespace DotTja.Types.Builders;

using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Enums;
using Exceptions;

internal class MetadataBuilder
{
    private LocalizedStringBuilder Title { get; } = new();
    private LocalizedStringBuilder Subtitle { get; } = new();
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

    public Metadata ToMetadata() => new(
        this.Title.ToLocalizedString(),
        this.Subtitle.ToLocalizedString()
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

    private static ImmutableDictionary<string, PropertyInfo> properties =
        typeof(MetadataBuilder)
            .GetProperties()
            .ToImmutableDictionary(p => p.Name.ToUpperInvariant(), p => p);

    public void Set(
        string key,
        string rawValue,
        PropertyInfo? propertyInfo = null,
        object? owner = null,
        Func<string, object>? customParser = null
    )
    {
        propertyInfo ??= properties.GetValueOrDefault(key)
                         ?? throw new ArgumentException($"Can't find property for key '{key}'.", nameof(key));
        owner ??= this;

        var existingValue = propertyInfo.GetValue(owner);
        if (existingValue != null)
        {
            throw new DuplicateKeyException(key, existingValue, rawValue);
        }

        object parsedValue;

        var propertyType = propertyInfo.PropertyType;
        if (customParser != null)
        {
            parsedValue = customParser(rawValue);
        }
        else if (propertyType == typeof(string))
        {
            parsedValue = rawValue;
        }
        else if (propertyType == typeof(int))
        {
            if (int.TryParse(rawValue, out var result))
            {
                parsedValue = result;
            }
            else
            {
                throw new ParsingException($"Unable to parse value '{rawValue}' for key '{key}' as int.");
            }
        }
        else if (propertyType == typeof(double))
        {
            if (double.TryParse(rawValue, out var result))
            {
                parsedValue = result;
            }
            else
            {
                throw new ParsingException($"Unable to parse value '{rawValue}' for key '{key}' as double.");
            }
        }
        else if (propertyType == typeof(FileInfo))
        {
            parsedValue = new FileInfo(rawValue);
        }
        else if (propertyType == typeof(TaikoWebSkin))
        {
            var pairs = rawValue
                .Split(",")
                .Select(
                    rawPair =>
                    {
                        var split = rawPair.Split(' ', 2);
                        if (split.Length != 2)
                        {
                            throw new ParsingException($"TaikoWebSkin missing space separator: {rawPair}");
                        }

                        return split;
                    }
                )
                .ToImmutableDictionary(p => p[0], p => p[1]);

            parsedValue = new TaikoWebSkin(
                new DirectoryInfo(pairs["dir"]),
                pairs["name"],
                pairs.GetValueOrDefault("song"),
                pairs.GetValueOrDefault("stage"),
                pairs.GetValueOrDefault("don")
            );
        }
        else if (propertyType.IsEnum)
        {
            if (Enum.TryParse(propertyType, rawValue, out var result))
            {
                Debug.Assert(result != null, nameof(result) + " != null");
                parsedValue = result;
            }
            else
            {
                throw new ParsingException($"Unable to parse value '{rawValue}' for key '{key}' as Enum of type '{propertyType.Name}'.");
            }
        }
        else
        {
            throw new ParsingException(
                $"Internal error: attempted to set value of property with type '{propertyType} "
                + "but no code was implemented for this type."
            );
        }

        propertyInfo.SetValue(owner, parsedValue);
    }

    public void SetValue(string key, string rawValue)
    {
        switch (key)
        {
            case "TITLE":
                if (this.Title.Default != null)
                {
                    throw new DuplicateKeyException(key, this.Title.Default, rawValue);
                }

                this.Title.Default = rawValue;
                break;
            case "TITLEJA":
                Debug.Assert(this.Title.Ja == null);
                this.Title.Ja = rawValue;
                break;
            case "TITLEEN":
                Debug.Assert(this.Title.En == null);
                this.Title.En = rawValue;
                break;
            case "TITLECN":
                Debug.Assert(this.Title.Cn == null);
                this.Title.Cn = rawValue;
                break;
            case "TITLETW":
                Debug.Assert(this.Title.Tw == null);
                this.Title.Tw = rawValue;
                break;
            case "TITLEKO":
                Debug.Assert(this.Title.Ko == null);
                this.Title.Ko = rawValue;
                break;

            case "SUBTITLE":
                Debug.Assert(this.Subtitle.Default == null);
                this.Subtitle.Default = rawValue;
                break;
            case "SUBTITLEJA":
                Debug.Assert(this.Subtitle.Ja == null);
                this.Subtitle.Ja = rawValue;
                break;
            case "SUBTITLEEN":
                Debug.Assert(this.Subtitle.En == null);
                this.Subtitle.En = rawValue;
                break;
            case "SUBTITLECN":
                Debug.Assert(this.Subtitle.Cn == null);
                this.Subtitle.Cn = rawValue;
                break;
            case "SUBTITLETW":
                Debug.Assert(this.Subtitle.Tw == null);
                this.Subtitle.Tw = rawValue;
                break;
            case "SUBTITLEKO":
                Debug.Assert(this.Subtitle.Ko == null);
                this.Subtitle.Ko = rawValue;
                break;

            case "BPM":
                Debug.Assert(this.Bpm == null);
                this.Bpm = int.Parse(rawValue, CultureInfo.InvariantCulture);
                break;
            case "WAVE":
                this.Wave = new FileInfo(rawValue);
                break;
            case "OFFSET":
                this.Offset = double.Parse(rawValue, CultureInfo.InvariantCulture);
                break;
            case "DEMOSTART":
                this.DemoStart = double.Parse(rawValue, CultureInfo.InvariantCulture);
                break;
            case "GENRE":
                this.Genre = rawValue;
                break;
            case "SCOREMODE":
                this.ScoreMode = Enum.Parse<ScoreMode>(rawValue);
                break;
            case "MAKER":
                this.Maker = rawValue;
                break;
            case "LYRICS":
                this.Lyrics = new FileInfo(rawValue);
                break;
            case "SONGVOL":
                this.SongVol = double.Parse(rawValue, CultureInfo.InvariantCulture);
                break;
            case "SEVOL":
                this.SeVol = double.Parse(rawValue, CultureInfo.InvariantCulture);
                break;
            case "SIDE":
                this.Side = rawValue switch
                {
                    "Normal" or "1" => Enums.Side.Normal,
                    "Ex" or "2" => Enums.Side.Ex,
                    "Both" or "3" => Enums.Side.Both,
                    _ => throw new ArgumentOutOfRangeException(nameof(rawValue), rawValue, null)
                };
                break;
            case "LIFE":
                this.Life = int.Parse(rawValue, CultureInfo.InvariantCulture);
                break;
            case "GAME":
                this.Game = Enum.Parse<Game>(rawValue);
                break;
            case "HEADSCROLL":
                this.HeadScroll = double.Parse(rawValue, CultureInfo.InvariantCulture);
                break;
            case "BGIMAGE":
                this.BgImage = new FileInfo(rawValue);
                break;
            case "BGMOVIE":
                this.BgMovie = new FileInfo(rawValue);
                break;
            case "MOVIEOFFSET":
                this.MovieOffset = double.Parse(rawValue, CultureInfo.InvariantCulture);
                break;
            case "TAIKOWEBSKIN":
                var pairs = rawValue
                    .Split(",")
                    .Select(
                        rawPair =>
                        {
                            var split = rawPair.Split(' ', 2);
                            if (split.Length != 2)
                            {
                                throw new ParsingException($"TaikoWebSkin missing space separator: {rawPair}");
                            }

                            return split;
                        }
                    )
                    .ToImmutableDictionary(p => p[0], p => p[1]);

                this.TaikoWebSkin = new TaikoWebSkin(
                    new DirectoryInfo(pairs["dir"]),
                    pairs["name"],
                    pairs.GetValueOrDefault("song"),
                    pairs.GetValueOrDefault("stage"),
                    pairs.GetValueOrDefault("don")
                );

                break;
            default:
                throw new ParsingException($"I don't know how to handle key '{key}' as part of main metadata.");
        }
    }
}
