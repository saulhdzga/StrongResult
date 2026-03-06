using StrongResult.Common;
using StrongResult.Generic;

namespace StrongResult.Test.Generic;

public class ResultTOnSuccessTests
{
    [Fact]
    public void OnSuccess_ShouldInvokeAction_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        bool called = false;
        result.OnSuccess(_ => called = true);
        Assert.True(called);
    }

    [Fact]
    public void OnSuccess_ShouldNotInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        bool called = false;
        result.OnSuccess(_ => called = true);
        Assert.False(called);
    }

    [Fact]
    public void OnSuccess_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        var result = Result<string>.Ok("abc");
        Assert.Throws<ArgumentNullException>(() => result.OnSuccess(null!));
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldInvokeAction_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        bool called = false;
        await result.OnSuccessAsync(async _ => { called = true; await Task.Delay(1); });
        Assert.True(called);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldNotInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        bool called = false;
        await result.OnSuccessAsync(async _ => { called = true; await Task.Delay(1); });
        Assert.False(called);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        var result = Result<string>.Ok("abc");
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.OnSuccessAsync(null!).AsTask());
    }

    [Fact]
    public async Task OnSuccessAsync_ValueTaskSource_WithSyncAction_ShouldExecuteOnSuccess()
    {
        var executed = false;
        var resultTask = new ValueTask<Result<int>>(Result<int>.Ok(5));
        var result = await resultTask.OnSuccessAsync(x => executed = true);
        Assert.True(executed);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task OnSuccessAsync_ValueTaskSource_WithAsyncAction_ShouldExecuteOnSuccess()
    {
        var executed = false;
        var resultTask = new ValueTask<Result<int>>(Result<int>.Ok(5));
        var result = await resultTask.OnSuccessAsync(async x =>
        {
            await Task.Yield();
            executed = true;
        });
        Assert.True(executed);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task OnSuccessAsync_TaskSource_WithSyncAction_ShouldNotExecuteOnFailure()
    {
        var executed = false;
        var error = Error.Create("E", "error");
        var resultTask = Task.FromResult(Result<int>.Fail(error));
        var result = await resultTask.OnSuccessAsync(x => executed = true);
        Assert.False(executed);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task OnSuccessAsync_TaskSource_WithAsyncAction_ShouldExecuteAndChain()
    {
        var executed = false;
        var resultTask = Task.FromResult(Result<int>.Ok(10));
        var result = await resultTask.OnSuccessAsync(async x =>
        {
            await Task.Yield();
            executed = true;
        });
        Assert.True(executed);
        Assert.True(result.IsSuccess);
        Assert.Equal(10, result.Value);
    }
}
