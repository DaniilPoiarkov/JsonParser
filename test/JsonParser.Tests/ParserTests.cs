using FluentAssertions;

namespace JsonParser.Tests;

public sealed class ParserTests
{
    [Fact]
    public void ParseNull()
    {
        var result = Parser.Parse("null");

        result.Should().BeNull();
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void ParseBool(string json, bool expected)
    {
        var result = Parser.Parse(json);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(" 1", 1)]
    [InlineData("0 ", 0)]
    [InlineData("69", 69)]
    [InlineData("420", 420)]
    public void ParseNumber(string json, int expected)
    {
        var result = Parser.Parse(json);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("\"one\"", "one")]
    [InlineData("\"hello world\"", "hello world")]
    [InlineData("\"69\"", "69")]
    [InlineData("\"true\"", "true")]
    [InlineData("\" \\\" wow!\\\" \"", " \" wow!\" ")]
    public void ParseString(string json, string expected)
    {
        var result = Parser.Parse(json);
        result.Should().Be(expected);
    }
}
