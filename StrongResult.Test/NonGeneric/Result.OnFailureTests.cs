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
}
