using StrongResult.Common;
using StrongResult.Generic;

namespace StrongResult.Test.Generic;

public class ResultTMapTests
{
    [Fact]
    public void Map_ShouldMapValue_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        var mapped = result.Map(s => s.Length);
        Assert.True(mapped.IsSuccess);
        Assert.Equal(3, mapped.Value);
    }

    [Fact]
    public void Map_ShouldPropagateError_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        var mapped = result.Map(s => s.Length);
        Assert.False(mapped.IsSuccess);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public void Map_ShouldThrowArgumentNullException_WhenFuncIsNull()
    {
        var result = Result<string>.Ok("abc");
        Assert.Throws<ArgumentNullException>(() => ResultTExtensions.Map<string, int>(result, null!));
    }

    [Fact]
    public async Task MapAsync_ShouldMapValue_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        var mapped = await result.MapAsync(async s => { await Task.Delay(1); return s.Length; });
        Assert.True(mapped.IsSuccess);
        Assert.Equal(3, mapped.Value);
    }

    [Fact]
    public async Task MapAsync_ShouldPropagateError_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        var mapped = await result.MapAsync(async s => { await Task.Yield(); return s.Length; });
        Assert.False(mapped.IsSuccess);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public async Task MapAsync_ShouldThrowArgumentNullException_WhenFuncIsNull()
    {
        var result = Result<string>.Ok("abc");
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.MapAsync<string, int>(null!).AsTask());
    }
}
