using FluentAssertions;

namespace JsonParser.Tests;

public class LexerTests
{
    [Fact]
    public void IsValid_WhenInputStringIsValid_ThenReturnsTrue()
    {
        var json = "{}";

        var lexer = new Lexer(json);

        var result = lexer.IsValid();

        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WhenInputStringIsInvalid_ThenReturnsFalse()
    {
        var jsonLPar = "{";
        var jsonRPar = "}";

        var lexer = new Lexer(jsonLPar);
        var result = lexer.IsValid();

        result.Should().BeFalse();

        lexer = new Lexer(jsonRPar);

        result = lexer.IsValid();
        result.Should().BeFalse();
    }
}
