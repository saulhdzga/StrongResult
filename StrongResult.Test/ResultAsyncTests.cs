using StrongResult.Common;
using StrongResult.NonGeneric;

namespace StrongResult.Test;

public class ResultAsyncTests
{
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
    public async Task OnWarningsAsync_ShouldInvokeAction_WhenWarningsExist()
    {
        var warning = Warning.Create("W1", "warn");
        var result = Result.PartialSuccess(warning);
        IReadOnlyList<IWarning>? received = null;
        await result.OnWarningsAsync(async w => { received = w; await Task.Delay(1); });
        Assert.NotNull(received);
        Assert.Contains(warning, received!);
    }

    [Fact]
    public async Task OnWarningsAsync_ShouldNotInvokeAction_WhenNoWarnings()
    {
        var result = Result.Ok();
        bool called = false;
        await result.OnWarningsAsync(async w => { called = true; await Task.Delay(1); });
        Assert.False(called);
    }

    [Fact]
    public async Task ForEachWarningAsync_ShouldInvokeActionForEachWarning()
    {
        var w1 = Warning.Create("W1", "warn1");
        var w2 = Warning.Create("W2", "warn2");
        var result = Result.PartialSuccess(w1, w2);
        var warnings = new List<IWarning>();
        await result.ForEachWarningAsync(async w => { warnings.Add(w); await Task.Delay(1); });
        Assert.Contains(w1, warnings);
        Assert.Contains(w2, warnings);
        Assert.Equal(2, warnings.Count);
    }

    [Fact]
    public async Task ForEachWarningAsync_ShouldNotInvokeAction_WhenNoWarnings()
    {
        var result = Result.Ok();
        bool called = false;
        await result.ForEachWarningAsync(async w => { called = true; await Task.Delay(1); });
        Assert.False(called);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnOnSuccessValue_WhenSuccess()
    {
        var result = Result.Ok();
        var output = await result.MatchAsync(async r => await Task.FromResult("success"), async e => await Task.FromResult("failure"));
        Assert.Equal("success", output);
    }

    [Fact]
    public async Task MatchAsync_ShouldReturnOnFailureValue_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result.Fail(error);
        var output = await result.MatchAsync(async r => await Task.FromResult("success"), async e => await Task.FromResult("failure"));
        Assert.Equal("failure", output);
    }
}
