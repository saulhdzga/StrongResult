using StrongResult.Common;
using StrongResult.Generic;

namespace StrongResult.Test;

public class ResultTAsyncTests
{
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
    public async Task MatchAsync_ShouldReturnOnSuccessOrOnFailureure()
    {
        var ok = Result<string>.Ok("abc");
        var fail = Result<string>.Fail(Error.Create("E", "fail"));
        int okResult = await ok.MatchAsync(async s => await Task.FromResult(s.Length), async e => await Task.FromResult(-1));
        int failResult = await fail.MatchAsync(async s => await Task.FromResult(s.Length), async e => await Task.FromResult(-1));
        Assert.Equal(3, okResult);
        Assert.Equal(-1, failResult);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldInvokeAction_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        bool called = false;
        await result.OnSuccessAsync(async () => { called = true; await Task.Delay(1); });
        Assert.True(called);
    }

    [Fact]
    public async Task OnSuccessAsync_ShouldNotInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        bool called = false;
        await result.OnSuccessAsync(async () => { called = true; await Task.Delay(1); });
        Assert.False(called);
    }

    [Fact]
    public async Task OnFailureAsync_ShouldInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        IError? received = null;
        await result.OnFailureAsync(async e => { received = e; await Task.Delay(1); });
        Assert.Equal(error, received);
    }

    [Fact]
    public async Task OnFailureAsync_ShouldNotInvokeAction_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        bool called = false;
        await result.OnFailureAsync(async e => { called = true; await Task.Delay(1); });
        Assert.False(called);
    }

    [Fact]
    public async Task OnWarningsAsync_ShouldInvokeAction_WhenWarningsExist()
    {
        var warning = Warning.Create("W1", "warn");
        var result = Result<string>.PartialSuccess("abc", warning);
        IReadOnlyList<IWarning>? received = null;
        await result.OnWarningsAsync(async w => { received = w; await Task.Delay(1); });
        Assert.NotNull(received);
        Assert.Contains(warning, received!);
    }

    [Fact]
    public async Task OnWarningsAsync_ShouldNotInvokeAction_WhenNoWarnings()
    {
        var result = Result<string>.Ok("abc");
        bool called = false;
        await result.OnWarningsAsync(async w => { called = true; await Task.Delay(1); });
        Assert.False(called);
    }

    [Fact]
    public async Task ForEachWarningAsync_ShouldInvokeActionForEachWarning()
    {
        var w1 = Warning.Create("W1", "warn1");
        var w2 = Warning.Create("W2", "warn2");
        var result = Result<string>.PartialSuccess("abc", w1, w2);
        var warnings = new List<IWarning>();
        await result.ForEachWarningAsync(async w => { warnings.Add(w); await Task.Delay(1); });
        Assert.Contains(w1, warnings);
        Assert.Contains(w2, warnings);
        Assert.Equal(2, warnings.Count);
    }

    [Fact]
    public async Task ForEachWarningAsync_ShouldNotInvokeAction_WhenNoWarnings()
    {
        var result = Result<string>.Ok("abc");
        bool called = false;
        await result.ForEachWarningAsync(async w => { called = true; await Task.Delay(1); });
        Assert.False(called);
    }
}
