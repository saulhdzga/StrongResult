using StrongResult.Common;
using StrongResult.NonGeneric;

namespace StrongResult.Test.NonGeneric;

public class ResultOnSuccessTests
{
    [Fact]
    public void OnSuccess_ShouldInvokeAction_WhenSuccess()
    {
        var result = Result.Ok();
        bool called = false;
        result.OnSuccess(_ => called = true);
        Assert.True(called);
    }

    [Fact]
    public void OnSuccess_ShouldNotInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result.Fail(error);
        bool called = false;
        result.OnSuccess(_ => called = true);
        Assert.False(called);
    }

    [Fact]
    public void OnSuccess_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        var result = Result.Ok();
        Assert.Throws<ArgumentNullException>(() => result.OnSuccess(null!));
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldInvokeAction_WhenSuccess()
    {
        var result = Result.Ok();
        bool called = false;
        await result.OnSuccessAsync(async _ => { called = true; await Task.Delay(1); });
        Assert.True(called);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldNotInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result.Fail(error);
        bool called = false;
        await result.OnSuccessAsync(async _ => { called = true; await Task.Delay(1); });
        Assert.False(called);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        var result = Result.Ok();
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.OnSuccessAsync(null!).AsTask());
    }
}
