using Microsoft.Extensions.Configuration;
using Xunit;

namespace Extensions.Configuration.Tests;

public class ConfigurationFilesTests
{
    [Fact]
    public void With_HasJson_When_Json_Present_Then_True()
    {
        ConfigurationFiles sut = new() { Json = new [] { "A", "B"} };
        Assert.True(sut.HasJson());
    }

    [Fact]
    public void With_HasJson_When_Json_Not_Present_Then_False()
    {
        ConfigurationFiles sut = new();
        Assert.False(sut.HasJson());
    }
}