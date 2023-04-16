namespace DotTja.Tests;

using Types;
using Types.Enums;
using Xunit.Abstractions;
using FluentAssertions;

public class UnitTest1
{
    private readonly ITestOutputHelper testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper) =>
        this.testOutputHelper = testOutputHelper;

    [Fact]
    public void TryParseAllTestFiles()
    {
        var dir = new DirectoryInfo("../../../TestTjas");
        foreach (var file in dir.EnumerateFiles())
        {
            using var reader = file.OpenText();
            DotTja.Deserialize(reader);
        }
    }

    [Fact]
    public void Test2()
    {
        AssertionOptions.AssertEquivalencyUsing(
            options => options
                // .ComparingByValue<DirectoryInfo>()
                // .ComparingByValue<FileInfo>()
                .ComparingRecordsByValue()
        );

        using var reader = new StreamReader("../../../TestTjas/Colorful Voice (Modified Metadata).tja");
        var parsed = DotTja.Deserialize(reader);

        parsed.Metadata.Title.Should().Be(
            new LocalizedString
            {
                Default = "Colorful Voice", Ja = "カラフルボイス", En = "Beans", Cn = "豆子繁體"
            }
        );
        parsed.Metadata.Subtitle.Should().Be(
            new LocalizedString
            {
                Default = "--cosMo@bousouP feat. Hatsune Miku & GUMI",
                Ja = "cosMo@暴走P feat.初音ミク・GUMI"
            }
        );
        parsed.Metadata.Bpm.Should().Be(240);
        parsed.Metadata.Wave.Should().Be(new FileInfo("Colorful Voice.ogg"));
        parsed.Metadata.Offset.Should().Be(-2.169);
        parsed.Metadata.DemoStart.Should().Be(44.158);
        parsed.Metadata.Genre.Should().Be("Oranges");
        parsed.Metadata.ScoreMode.Should().Be(ScoreMode.AcGen0);
        parsed.Metadata.Maker.Should().Be("mom");
        parsed.Metadata.Lyrics.Should().Be(new FileInfo("熱情のスペクトラム.vtt"));
        parsed.Metadata.SongVol.Should().Be(2003);
        parsed.Metadata.SeVol.Should().Be(45);
        parsed.Metadata.Side.Should().Be(Side.Ex);
        parsed.Metadata.Life.Should().Be(10023);
        parsed.Metadata.Game.Should().Be(Game.Jube);
        parsed.Metadata.HeadScroll.Should().Be(0.2);
        parsed.Metadata.BgImage.Should().Be(new FileInfo("123.png"));
        parsed.Metadata.MovieOffset.Should().Be(1.5);
        parsed.Metadata.BgMovie.Should().Be(new FileInfo("Colorful Voice.mp4"));
        parsed.Metadata.TaikoWebSkin.Should().Be(
            new TaikoWebSkin(
                new DirectoryInfo("../song_skins"),
                "miku",
                "static",
                "none",
                "fastscroll"
            )
        );
    }
}
