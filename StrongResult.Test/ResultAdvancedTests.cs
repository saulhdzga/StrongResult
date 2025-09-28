using StrongResult.Common;
using StrongResult.NonGeneric;

namespace StrongResult.Test;

public class ResultAdvancedTests
{
    [Fact]
    public void OnSuccess_ShouldInvokeAction_WhenSuccess()
    {
        var result = Result.Ok();
        bool called = false;
        result.OnSuccess(() => called = true);
        Assert.True(called);
    }

    [Fact]
    public void OnSuccess_ShouldNotInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result.Fail(error);
        bool called = false;
        result.OnSuccess(() => called = true);
        Assert.False(called);
    }

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
    public void OnWarnings_ShouldInvokeAction_WhenWarningsExist()
    {
        var warning = Warning.Create("W1", "warn");
        var result = Result.PartialSuccess(warning);
        IReadOnlyList<IWarning>? received = null;
        result.OnWarnings(w => received = w);
        Assert.NotNull(received);
        Assert.Contains(warning, received!);
    }

    [Fact]
    public void OnWarnings_ShouldNotInvokeAction_WhenNoWarnings()
    {
        var result = Result.Ok();
        bool called = false;
        result.OnWarnings(w => called = true);
        Assert.False(called);
    }

    [Fact]
    public void ForEachWarning_ShouldInvokeActionForEachWarning()
    {
        var w1 = Warning.Create("W1", "warn1");
        var w2 = Warning.Create("W2", "warn2");
        var result = Result.PartialSuccess(w1, w2);
        var warnings = new List<IWarning>();
        result.ForEachWarning(w => warnings.Add(w));
        Assert.Contains(w1, warnings);
        Assert.Contains(w2, warnings);
        Assert.Equal(2, warnings.Count);
    }

    [Fact]
    public void ForEachWarning_ShouldNotInvokeAction_WhenNoWarnings()
    {
        var result = Result.Ok();
        bool called = false;
        result.ForEachWarning(w => called = true);
        Assert.False(called);
    }
}
