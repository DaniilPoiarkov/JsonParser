using FluentAssertions;

namespace JsonParser.Tests;

public sealed class LexerTests
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

    [Theory]
    [InlineData("{}")]
    [InlineData("{ }")]
    public void Analyse_WhenJsonValid_ThenProperTokensReturned(string json)
    {
        var expectedTokens = 2;

        var lexer = new Lexer(json);

        var tokens = lexer.Analyse();

        tokens.Should().HaveCount(expectedTokens);
    }
}
