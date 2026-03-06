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
    public void OnWarnings_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        var result = Result.PartialSuccess(Warning.Create("W", "warn"));
        Assert.Throws<ArgumentNullException>(() => result.OnWarnings(null!));
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

    [Fact]
    public void ForEachWarning_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        var result = Result.PartialSuccess(Warning.Create("W", "warn"));
        Assert.Throws<ArgumentNullException>(() => result.ForEachWarning(null!));
    }

    [Fact]
    public void Match_ShouldReturnOnSuccessValue_WhenSuccess()
    {
        var result = Result.Ok();
        var output = result.Match(r => "success", e => "failure");
        Assert.Equal("success", output);
    }

    [Fact]
    public void Match_ShouldReturnOnFailureValue_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result.Fail(error);
        var output = result.Match(r => "success", e => "failure");
        Assert.Equal("failure", output);
    }

    [Fact]
    public void Match_ShouldThrowArgumentNullException_WhenOnSuccessIsNull()
    {
        var result = Result.Ok();
        Assert.Throws<ArgumentNullException>(() => result.Match<string>(null!, e => "failure"));
    }

    [Fact]
    public void Match_ShouldThrowArgumentNullException_WhenOnFailureIsNull()
    {
        var result = Result.Ok();
        Assert.Throws<ArgumentNullException>(() => result.Match(r => "success", null!));
    }
}
