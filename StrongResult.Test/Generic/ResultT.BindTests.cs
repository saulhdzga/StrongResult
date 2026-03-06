using StrongResult.Common;
using StrongResult.Generic;

namespace StrongResult.Test.Generic;

public class ResultTBindTests
{
    [Fact]
    public void Bind_ShouldChainResults()
    {
        var result = Result<string>.Ok("abc");
        var bound = result.Bind(s => Result<int>.Ok(s.Length));
        Assert.True(bound.IsSuccess);
        Assert.Equal(3, bound.Value);
    }

    [Fact]
    public void Bind_ShouldPropagateError_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        var bound = result.Bind(s => Result<int>.Ok(s.Length));
        Assert.False(bound.IsSuccess);
        Assert.Equal(error, bound.Error);
    }

    [Fact]
    public void Bind_ShouldThrowArgumentNullException_WhenFuncIsNull()
    {
        var result = Result<string>.Ok("abc");
        Assert.Throws<ArgumentNullException>(() => ResultTExtensions.Bind<string, string>(result, null!));
    }

    [Fact]
    public async Task BindAsync_ShouldChainResults()
    {
        var result = Result<string>.Ok("abc");
        var bound = await result.BindAsync(async s => { await Task.Delay(1); return Result<int>.Ok(s.Length); });
        Assert.True(bound.IsSuccess);
        Assert.Equal(3, bound.Value);
    }

    [Fact]
    public async Task BindAsync_ShouldPropagateError_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        var bound = await result.BindAsync(async s => { await Task.Yield(); return Result<int>.Ok(s.Length); });
        Assert.False(bound.IsSuccess);
        Assert.Equal(error, bound.Error);
    }

    [Fact]
    public async Task BindAsync_ShouldThrowArgumentNullException_WhenFuncIsNull()
    {
        var result = Result<string>.Ok("abc");
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.BindAsync<string, int>(null!).AsTask());
    }
}
