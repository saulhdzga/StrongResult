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

    [Fact]
    public async Task BindAsync_ValueTaskSource_WithSyncFunc_ShouldBindValue()
    {
        var resultTask = new ValueTask<Result<int>>(Result<int>.Ok(5));
        var bound = await resultTask.BindAsync(x => Result<string>.Ok(x.ToString()));
        Assert.True(bound.IsSuccess);
        Assert.Equal("5", bound.Value);
    }

    [Fact]
    public async Task BindAsync_ValueTaskSource_WithAsyncFunc_ShouldBindValue()
    {
        var resultTask = new ValueTask<Result<int>>(Result<int>.Ok(5));
        var bound = await resultTask.BindAsync(async x => await ValueTask.FromResult(Result<string>.Ok(x.ToString())));
        Assert.True(bound.IsSuccess);
        Assert.Equal("5", bound.Value);
    }

    [Fact]
    public async Task BindAsync_TaskSource_WithSyncFunc_ShouldBindValue()
    {
        var resultTask = Task.FromResult(Result<int>.Ok(5));
        var bound = await resultTask.BindAsync(x => Result<string>.Ok(x.ToString()));
        Assert.True(bound.IsSuccess);
        Assert.Equal("5", bound.Value);
    }

    [Fact]
    public async Task BindAsync_TaskSource_WithAsyncFunc_ShouldBindValue()
    {
        var resultTask = Task.FromResult(Result<int>.Ok(5));
        var bound = await resultTask.BindAsync(async x => await ValueTask.FromResult(Result<string>.Ok(x.ToString())));
        Assert.True(bound.IsSuccess);
        Assert.Equal("5", bound.Value);
    }

    [Fact]
    public async Task BindAsync_ValueTaskSourceFailure_ShouldPreserveError()
    {
        var error = Error.Create("E", "error");
        var resultTask = new ValueTask<Result<int>>(Result<int>.Fail(error));
        var bound = await resultTask.BindAsync(x => Result<string>.Ok(x.ToString()));
        Assert.False(bound.IsSuccess);
        Assert.Equal(error, bound.Error);
    }

    [Fact]
    public async Task BindAsync_WithWarningsCombination_ShouldCombineWarnings()
    {
        var warning1 = Warning.Create("W1", "warning1");
        var warning2 = Warning.Create("W2", "warning2");
        var resultTask = Task.FromResult(Result<int>.PartialSuccess(5, warning1));
        var bound = await resultTask.BindAsync(x => Result<string>.PartialSuccess(x.ToString(), warning2));
        Assert.True(bound.IsSuccess);
        Assert.Equal("5", bound.Value);
        Assert.Equal(2, bound.Warnings.Count);
    }
}
