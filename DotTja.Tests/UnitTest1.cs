namespace DotTja.Tests;

using Types;
using Types.Enums;
using Xunit.Abstractions;
using FluentAssertions;

public class UnitTest1
{
    private readonly ITestOutputHelper testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;

        AssertionOptions.AssertEquivalencyUsing(
            options => options
                .Excluding(info => info.DeclaringType == typeof(FileInfo) && info.Name == "Length")
                .Excluding(info => info.DeclaringType == typeof(DirectoryInfo) && info.Name != "FullName")
        );
    }

    [Fact]
    public void TryParseAllTestFiles()
    {
        var dir = new DirectoryInfo("../../../TestTjas");
        foreach (var file in dir.EnumerateFiles())
        {
            using var reader = file.OpenText();
            var parsed = DotTja.Deserialize(reader);
            this.testOutputHelper.WriteLine(parsed.ToString());
        }
    }

    [Fact]
    public void Test2()
    {
        using var reader = new StreamReader("../../../TestTjas/Colorful Voice (Modified Metadata).tja");
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
                    new DirectoryInfo("../song_skinsd"),
                    "miku",
                    "static",
                    "none",
                    "fastscroll"
                )
            }
        );
    }
}
