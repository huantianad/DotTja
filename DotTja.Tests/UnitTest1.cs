namespace DotTja.Tests;

using Xunit.Abstractions;

public class UnitTest1
{
    private readonly ITestOutputHelper testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper) =>
        this.testOutputHelper = testOutputHelper;

    [Fact]
    public void Test1()
    {
        using var reader = new StreamReader("../../../test.tja");
        var parsed = DotTja.Deserialize(reader);
        this.testOutputHelper.WriteLine(parsed.ToString());
    }
}
