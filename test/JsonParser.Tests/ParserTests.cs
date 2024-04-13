using FluentAssertions;

namespace JsonParser.Tests;

public sealed class ParserTests
{
    [Fact]
    public void Parse_WhenValueIsNull_ThenNullReturned()
    {
        var result = Parser.Parse("null");

        result.Should().BeNull();
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    public void Parse_WhenBooleanValueParsed_ThenItParsedProprly(string json, bool expected)
    {
        var result = Parser.Parse(json);
        result.Should().Be(expected);
    }
}
