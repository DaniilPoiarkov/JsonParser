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

    [Theory]
    [InlineData("[true, false]", true, false)]
    [InlineData("[1, 2, 69, 420]", 1, 2, 69, 420)]
    [InlineData("[\"hello\", \" \\\"world!\\\" \"]", "hello", " \"world!\" ")]
    [InlineData("[]")]
    [InlineData("[1, false, null, \"\"]", 1, false, null, "")]
    public void ParseArray(string json, params object?[] expectedValues)
    {
        var result = Parser.Parse(json).As<object?[]>();

        result.Should().NotBeNull();

        result.Length.Should().Be(expectedValues.Length);

        for (int i = 0; i < result.Length; i++)
        {
            var value = result[i];
            var expected = expectedValues[i];
            value.Should().BeEquivalentTo(expected);
        }
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{ \"hello\" : \"world\" }", "hello", "world")]
    [InlineData("{ \"name\" : \"David\", \"surname\": \"Bowie\" }", "name", "David", "surname", "Bowie")]
    [InlineData("{ \"age\" : 69, \"time\": 420 }", "age", 69, "time", 420)]
    public void ParseObject(string json, params object[] properties)
    {
        var result = Parser.Parse(json).As<Dictionary<string, object>>();

        result.Should().NotBeNull();

        var paired = properties.Chunk(2)
            .ToArray();

        for (int i = 0; i < paired.Length; i++)
        {
            var expected = paired[i];
            var value = result[expected[0].ToString()!];
            value.Should().BeEquivalentTo(expected[1]);
        }
    }

    [Fact]
    public void ParseObjectWithNestedArray()
    {
        var json = "{ \"ids\": [1, 2, 3],\n\"finishAt\": null }";

        var result = Parser.Parse(json).As<Dictionary<string, object?>>();

        result.Should().NotBeNull();

        var idsArray = result["ids"].As<object[]>()
            .Cast<decimal>();

        idsArray.Should().NotBeNull();
        idsArray.SequenceEqual([1, 2, 3]).Should().BeTrue();

        result["finishAt"].Should().BeNull();

        result.Count.Should().Be(2);
    }

    [Fact]
    public void ParseComplexObject()
    {
        var json = """
            {
                "id": "43783789-adb0-43db-b79c-fd7f88b75158",
                "name": "Marcus",
                "surname": "Aurelius",
                "quote": " I once said: \"When you arise in the morning, think of what a precious privilege it is to be alive - to breathe, to think, to enjoy, to love.\"",
                "age": 21,
                "roles": [ "emperor", "stoic", "warior" ],
                "isAlive": false,
                "mistakes": null,
                "thoughts": [
                    {
                        "id": 1,
                        "description": "veni"
                    },
                    {
                        "id": 2,
                        "description": "vidi"
                    },
                    {
                        "id": 3,
                        "description": "vici"
                    }
                ]
            }
            """;

        var expectedResult = new Dictionary<string, object?>()
        {
            ["id"] = "43783789-adb0-43db-b79c-fd7f88b75158",
            ["name"] = "Marcus",
            ["surname"] = "Aurelius",
            ["quote"] = " I once said: \"When you arise in the morning, think of what a precious privilege it is to be alive - to breathe, to think, to enjoy, to love.\"",
            ["age"] = 21,
            ["roles"] = new string[] { "emperor", "stoic", "warior" },
            ["isAlive"] = false,
            ["mistakes"] = null,
            ["thoughts"] = new object[]
            {
                new Dictionary<string, object>
                {
                    ["id"] = 1,
                    ["description"] = "veni"
                },
                new Dictionary<string, object>
                {
                    ["id"] = 2,
                    ["description"] = "vidi"
                },
                new Dictionary<string, object>
                {
                    ["id"] = 3,
                    ["description"] = "vici"
                },
            }
        };

        var result = Parser.Parse(json).As<Dictionary<string, object?>>();

        result.Should().NotBeNullOrEmpty();

        result.Count.Should().Be(expectedResult.Count);

        string thoughts = "thoughts";

        foreach (var kv in expectedResult)
        {
            if (kv.Key == thoughts)
            {
                continue;
            }

            if (kv.Value is object[] arr)
            {
                result[kv.Key].As<object[]>().SequenceEqual(arr).Should().BeTrue();
                continue;
            }

            result[kv.Key].Should().Be(kv.Value);
        }

        var expectedThoughts = expectedResult[thoughts].As<object[]>();
        var actualThoughts = result[thoughts].As<object[]>();

        actualThoughts.Length.Should().Be(expectedThoughts.Length);

        for (int i = 0; i < expectedThoughts.Length; i++)
        {
            var expected = expectedThoughts[i];
            var actual = actualThoughts[i];

            expected.Should().BeEquivalentTo(actual);
        }
    }

    [Theory]
    [InlineData("}")]
    [InlineData("{\"name\":}")]
    [InlineData("")]
    [InlineData("...")]
    [InlineData("   ")]
    public void ThrowsException_WhenJsonInvalid(string json)
    {
        var action = () => Parser.Parse(json);

        action.Should().ThrowExactly<ArgumentException>();
    }
}
