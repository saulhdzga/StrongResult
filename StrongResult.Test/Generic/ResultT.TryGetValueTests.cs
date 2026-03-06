using StrongResult.Common;
using StrongResult.Generic;

namespace StrongResult.Test.Generic;

public class ResultTTryGetValueTests
{
    [Fact]
    public void TryGetValue_ShouldReturnTrueAndValue_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        var success = result.TryGetValue(out var value);
        Assert.True(success);
        Assert.Equal("abc", value);
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalseAndDefault_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        var success = result.TryGetValue(out var value);
        Assert.False(success);
        Assert.Null(value);
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrueAndValue_WhenPartialSuccess()
    {
        var warning = Warning.Create("W1", "warn");
        var result = Result<string>.PartialSuccess("abc", warning);
        var success = result.TryGetValue(out var value);
        Assert.True(success);
        Assert.Equal("abc", value);
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalseAndDefault_WhenControlledError()
    {
        var error = Error.Create("E", "fail");
        var warning = Warning.Create("W1", "warn");
        var result = Result<string>.ControlledError(error, warning);
        var success = result.TryGetValue(out var value);
        Assert.False(success);
        Assert.Null(value);
    }

    [Fact]
    public void ImplicitOperator_ShouldReturnValue_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        string value = result;
        Assert.Equal("abc", value);
    }

    [Fact]
    public void ImplicitOperator_ShouldThrow_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        Assert.Throws<InvalidOperationException>(() => { string _ = result; });
    }
}
