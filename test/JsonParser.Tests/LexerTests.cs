using FluentAssertions;

namespace JsonParser.Tests;

public class LexerTests
{
    [Theory]
    [InlineData("{}")]
    [InlineData("{ }")]
    public void IsValid_WhenInputStringIsValid_ThenReturnsTrue(string json)
    {
        var lexer = new Lexer(json);

        var result = lexer.IsValid();

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("{")]
    [InlineData("}")]
    public void IsValid_WhenInputStringIsInvalid_ThenReturnsFalse(string json)
    {
        var lexer = new Lexer(json);
        var result = lexer.IsValid();

        result.Should().BeFalse();

        lexer = new Lexer(json);

        result = lexer.IsValid();
        result.Should().BeFalse();
    }
}
