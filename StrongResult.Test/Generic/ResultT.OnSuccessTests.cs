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
}
