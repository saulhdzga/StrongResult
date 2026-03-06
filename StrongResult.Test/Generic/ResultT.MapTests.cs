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

    [Fact]
    public async Task MapAsync_ValueTaskSource_WithSyncFunc_ShouldMapValue()
    {
        var resultTask = new ValueTask<Result<int>>(Result<int>.Ok(5));
        var mapped = await resultTask.MapAsync(x => x * 2);
        Assert.True(mapped.IsSuccess);
        Assert.Equal(10, mapped.Value);
    }

    [Fact]
    public async Task MapAsync_ValueTaskSource_WithAsyncFunc_ShouldMapValue()
    {
        var resultTask = new ValueTask<Result<int>>(Result<int>.Ok(5));
        var mapped = await resultTask.MapAsync(async x => await ValueTask.FromResult(x * 2));
        Assert.True(mapped.IsSuccess);
        Assert.Equal(10, mapped.Value);
    }

    [Fact]
    public async Task MapAsync_TaskSource_WithSyncFunc_ShouldMapValue()
    {
        var resultTask = Task.FromResult(Result<int>.Ok(5));
        var mapped = await resultTask.MapAsync(x => x * 2);
        Assert.True(mapped.IsSuccess);
        Assert.Equal(10, mapped.Value);
    }

    [Fact]
    public async Task MapAsync_TaskSource_WithAsyncFunc_ShouldMapValue()
    {
        var resultTask = Task.FromResult(Result<int>.Ok(5));
        var mapped = await resultTask.MapAsync(async x => await ValueTask.FromResult(x * 2));
        Assert.True(mapped.IsSuccess);
        Assert.Equal(10, mapped.Value);
    }

    [Fact]
    public async Task MapAsync_ValueTaskSourceFailure_ShouldPreserveError()
    {
        var error = Error.Create("E", "error");
        var resultTask = new ValueTask<Result<int>>(Result<int>.Fail(error));
        var mapped = await resultTask.MapAsync(x => x * 2);
        Assert.False(mapped.IsSuccess);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public async Task MapAsync_WithWarnings_ShouldPreserveWarnings()
    {
        var warning = Warning.Create("W", "warning");
        var resultTask = Task.FromResult(Result<int>.PartialSuccess(5, warning));
        var mapped = await resultTask.MapAsync(x => x * 2);
        Assert.True(mapped.IsSuccess);
        Assert.Equal(10, mapped.Value);
        Assert.Single(mapped.Warnings);
    }
}
