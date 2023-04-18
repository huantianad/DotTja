namespace DotTja.Tests;

using Exceptions;
using FluentAssertions;
using Types;
using Types.Enums;
using Xunit.Abstractions;

public class ParserTest
{
    private readonly ITestOutputHelper testOutputHelper;

    public ParserTest(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;

        AssertionOptions.AssertEquivalencyUsing(
            options => options
                .Excluding(info => info.DeclaringType == typeof(FileInfo) && info.Name != "FullPath")
                .Excluding(info => info.DeclaringType == typeof(DirectoryInfo) && info.Name != "FullPath")
        );
    }

    public static StreamReader TestFile(string name) => new($"../../../TestTjas/{name}");
    public static StreamReader TestFileFailing(string name) => new($"../../../TestTjasFailing/{name}");

    public static IEnumerable<object[]> BundledFiles =>
        new DirectoryInfo("../../../TestTjas")
            .EnumerateFiles()
            .Select(file => new[] {file});

    [Theory]
    [MemberData(nameof(BundledFiles))]
    public void ParseAllBundledFiles(FileInfo file)
    {
        using var reader = file.OpenText();
        var parsed = DotTja.Deserialize(reader);
        this.testOutputHelper.WriteLine(parsed.ToString());
    }

    public static IEnumerable<object[]> EseTjaFiles =>
        new DirectoryInfo("/home/huantian/Games/ESE/")
            .EnumerateDirectories()
            .SelectMany(category => category.EnumerateDirectories())
            .SelectMany(songFolder => songFolder.EnumerateFiles())
            .Where(file => file.Extension == ".tja")
            .Select(file => new[] {file});

    [Theory]
    [MemberData(nameof(EseTjaFiles))]
    public void ParseAllEseFiles(FileInfo file)
    {
        using var reader = file.OpenText();
        DotTja.Deserialize(reader);
    }

    [Fact]
    public void EarlyEndExceptions()
    {
        using var reader = TestFileFailing("Colorful Voice (No Course).tja");
        reader.Invoking(DotTja.Deserialize).Should()
            .Throw<ParsingException>()
            .WithMessage("Encountered error while parsing at LineNumber = 14, CurrentLine = ''.")
            .WithInnerException<ParsingException>()
            .WithMessage("Encountered end of stream when parsing metadata.");
    }

    [Fact]
    public void CheckSpecific()
    {
        using var reader = TestFile("Colorful Voice (Modified Metadata).tja");
        var parsed = DotTja.Deserialize(reader);

        parsed.Metadata.Should().BeEquivalentTo(
            new Metadata
            {
                Title = new LocalizedString
                {
                    Default = "Colorful Voice", Ja = "カラフルボイス", En = "Beans", Cn = "豆子繁體"
                },
                Subtitle = new LocalizedString
                {
                    Default = "--cosMo@bousouP feat. Hatsune Miku & GUMI", Ja = "cosMo@暴走P feat.初音ミク・GUMI"
                },
                Bpm = 240,
                Wave = new FileInfo("Colorful Voice.ogg"),
                Offset = -2.169,
                DemoStart = 44.158,
                Genre = "Oranges",
                ScoreMode = ScoreMode.AcGen0,
                Maker = "mom",
                Lyrics = new FileInfo("熱情のスペクトラム.vtt"),
                SongVol = 2003,
                SeVol = 45,
                Side = Side.Ex,
                Life = 10023,
                Game = Game.Jube,
                HeadScroll = 0.2,
                BgImage = new FileInfo("123.png"),
                MovieOffset = 1.5,
                BgMovie = new FileInfo("Colorful Voice.mp4"),
                TaikoWebSkin = new TaikoWebSkin(
                    new DirectoryInfo("../song_skins"),
                    "miku",
                    "static",
                    "none",
                    "fastscroll"
                )
            }
        );
    }
}
