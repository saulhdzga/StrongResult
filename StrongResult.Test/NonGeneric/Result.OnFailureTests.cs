using StrongResult.Common;
using StrongResult.NonGeneric;

namespace StrongResult.Test.NonGeneric;

public class ResultOnFailureTests
{
    [Fact]
    public void OnFailure_ShouldInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result.Fail(error);
        IError? received = null;
        result.OnFailure(e => received = e);
        Assert.Equal(error, received);
    }

    [Fact]
    public void OnFailure_ShouldNotInvokeAction_WhenSuccess()
    {
        var result = Result.Ok();
        bool called = false;
        result.OnFailure(e => called = true);
        Assert.False(called);
    }

    [Fact]
    public void OnFailure_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        var result = Result.Fail(Error.Create("E", "fail"));
        Assert.Throws<ArgumentNullException>(() => result.OnFailure(null!));
    }

    [Fact]
    public async Task OnFailureAsync_ShouldInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result.Fail(error);
        IError? received = null;
        await result.OnFailureAsync(async e => { received = e; await Task.Delay(1); });
        Assert.Equal(error, received);
    }

    [Fact]
    public async Task OnFailureAsync_ShouldNotInvokeAction_WhenSuccess()
    {
        var result = Result.Ok();
        bool called = false;
        await result.OnFailureAsync(async e => { called = true; await Task.Delay(1); });
        Assert.False(called);
    }

    [Fact]
    public async Task OnFailureAsync_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        var result = Result.Fail(Error.Create("E", "fail"));
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.OnFailureAsync(null!).AsTask());
    }

    [Fact]
    public async Task OnFailureAsync_ValueTaskSource_WithSyncAction_ShouldExecuteOnFailure()
    {
        var executed = false;
        var error = Error.Create("E", "error");
        var resultTask = new ValueTask<Result>(Result.Fail(error));
        var result = await resultTask.OnFailureAsync(e => executed = true);
        Assert.True(executed);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task OnFailureAsync_ValueTaskSource_WithAsyncAction_ShouldExecuteOnFailure()
    {
        var executed = false;
        var error = Error.Create("E", "error");
        var resultTask = new ValueTask<Result>(Result.Fail(error));
        var result = await resultTask.OnFailureAsync(async e =>
        {
            await Task.Yield();
            executed = true;
        });
        Assert.True(executed);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task OnFailureAsync_TaskSource_WithSyncAction_ShouldNotExecuteOnSuccess()
    {
        var executed = false;
        var resultTask = Task.FromResult(Result.Ok());
        var result = await resultTask.OnFailureAsync(e => executed = true);
        Assert.False(executed);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task OnFailureAsync_TaskSource_WithAsyncAction_ShouldCaptureError()
    {
        IError? capturedError = null;
        var error = Error.Create("E", "error");
        var resultTask = Task.FromResult(Result.Fail(error));
        var result = await resultTask.OnFailureAsync(async e =>
        {
            await Task.Yield();
            capturedError = e;
        });
        Assert.False(result.IsSuccess);
        Assert.Equal(error, capturedError);
    }
}
