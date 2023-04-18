namespace DotTja.Tests;

using EnumConverter;
using Exceptions;
using FluentAssertions;

public class EnumConverterTest
{
    public enum TestEnum
    {
        [EnumAlias("Foo", "owo", "1", "23")]
        Foo,
        [EnumAlias("Bar", "4")]
        Bar,
        [EnumAlias("5")]
        FooBar = 3
    }

    [Theory]
    [InlineData("Foo", TestEnum.Foo)]
    [InlineData("Bar", TestEnum.Bar)]
    [InlineData("Beans", null)]
    [InlineData("owo", TestEnum.Foo)]
    [InlineData("-1", null)]
    [InlineData("0", null)]
    [InlineData("1", TestEnum.Foo)]
    [InlineData("2", null)]
    [InlineData("3", null)]
    [InlineData("4", TestEnum.Bar)]
    [InlineData("5", TestEnum.FooBar)]
    [InlineData("6", null)]
    [InlineData("23", TestEnum.Foo)]
    public static void TestEnumParsing(string value, TestEnum? expectedValue)
    {
        var func = () => (TestEnum) EnumConverter.Parse(typeof(TestEnum), value);
        if (expectedValue != null)
        {
            func.Should().NotThrow();
            func().Should().Be(expectedValue);
        }
        else
        {
            func.Should().ThrowExactly<ParsingException>();
        }
    }

    [Theory]
    [InlineData(TestEnum.Foo, "Foo")]
    [InlineData(TestEnum.Bar, "Bar")]
    [InlineData(TestEnum.FooBar, "5")]
    [InlineData((TestEnum) 4, null)]
    public static void TestEnumSerialization(TestEnum value, string? expectedValue)
    {
        var func = () => EnumConverter.Serialize(value);
        if (expectedValue != null)
        {
            func.Should().NotThrow();
            func().Should().Be(expectedValue);
        }
        else
        {
            func.Should()
                .ThrowExactly<ArgumentOutOfRangeException>()
                .WithMessage("Value out of range for enum 'TestEnum'.*");
        }
    }

    public enum BadTestEnum
    {
        [EnumAlias("Foo")]
        Foo,
        Bar,
        FooBar
    }

    [Fact]
    public static void ParseBadEnum()
    {
        var parse = () => EnumConverter.Parse(typeof(BadTestEnum), "Foo");
        parse.Should()
            .Throw<MissingEnumAliasException>()
            .WithMessage("*BadTestEnum*Bar*");
    }

    [Fact]
    public static void SerializeBadEnum()
    {
        var serialize = () => EnumConverter.Serialize(BadTestEnum.Bar);
        serialize.Should()
            .Throw<MissingEnumAliasException>()
            .WithMessage("*BadTestEnum*Bar*");
    }
}
