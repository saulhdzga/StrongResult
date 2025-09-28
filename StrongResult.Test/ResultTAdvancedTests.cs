using StrongResult.Common;
using StrongResult.Generic;

namespace StrongResult.Test;

public class ResultTAdvancedTests
{
    [Fact]
    public void ImplicitOperator_ShouldReturnValue_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        string value = result; // implicit
        Assert.Equal("abc", value);
    }

    [Fact]
    public void ImplicitOperator_ShouldThrow_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        Assert.Throws<InvalidOperationException>(() => { string _ = result; });
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrueAndValue_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        var success = result.TryGetValue(out var value);
        Assert.True(success);
        Assert.Equal("abc", value);
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalseAndDefault_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        var success = result.TryGetValue(out var value);
        Assert.False(success);
        Assert.Null(value);
    }

    [Fact]
    public void Map_ShouldMapValue_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        var mapped = result.Map(s => s.Length);
        Assert.True(mapped.IsSuccess);
        Assert.Equal(3, mapped.Value);
    }

    [Fact]
    public void Map_ShouldPropagateError_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        var mapped = result.Map(s => s.Length);
        Assert.False(mapped.IsSuccess);
        Assert.Equal(error, mapped.Error);
    }

    [Fact]
    public void Map_ShouldThrowArgumentNullException_WhenFuncIsNull()
    {
        var result = Result<string>.Ok("abc");
        Assert.Throws<ArgumentNullException>(() => ResultTExtensions.Map<string, int>(result, null!));
    }

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
    public void Match_ShouldReturnOnSuccessOrOnFailureure()
    {
        var ok = Result<string>.Ok("abc");
        var fail = Result<string>.Fail(Error.Create("E", "fail"));
        int okResult = ok.Match(s => s.Length, e => -1);
        int failResult = fail.Match(s => s.Length, e => -1);
        Assert.Equal(3, okResult);
        Assert.Equal(-1, failResult);
    }

    [Fact]
    public void OnSuccess_ShouldInvokeAction_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        bool called = false;
        result.OnSuccess(() => called = true);
        Assert.True(called);
    }

    [Fact]
    public void OnSuccess_ShouldNotInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        bool called = false;
        result.OnSuccess(() => called = true);
        Assert.False(called);
    }

    [Fact]
    public void OnFailure_ShouldInvokeAction_WhenFailure()
    {
        var error = Error.Create("E", "fail");
        var result = Result<string>.Fail(error);
        IError? received = null;
        result.OnFailure(e => received = e);
        Assert.Equal(error, received);
    }

    [Fact]
    public void OnFailure_ShouldNotInvokeAction_WhenSuccess()
    {
        var result = Result<string>.Ok("abc");
        bool called = false;
        result.OnFailure(e => called = true);
        Assert.False(called);
    }

    [Fact]
    public void OnWarnings_ShouldInvokeAction_WhenWarningsExist()
    {
        var warning = Warning.Create("W1", "warn");
        var result = Result<string>.PartialSuccess("abc", warning);
        IReadOnlyList<IWarning>? received = null;
        result.OnWarnings(w => received = w);
        Assert.NotNull(received);
        Assert.Contains(warning, received!);
    }

    [Fact]
    public void OnWarnings_ShouldNotInvokeAction_WhenNoWarnings()
    {
        var result = Result<string>.Ok("abc");
        bool called = false;
        result.OnWarnings(w => called = true);
        Assert.False(called);
    }

    [Fact]
    public void ForEachWarning_ShouldInvokeActionForEachWarning()
    {
        var w1 = Warning.Create("W1", "warn1");
        var w2 = Warning.Create("W2", "warn2");
        var result = Result<string>.PartialSuccess("abc", w1, w2);
        var warnings = new List<IWarning>();
        result.ForEachWarning(w => warnings.Add(w));
        Assert.Contains(w1, warnings);
        Assert.Contains(w2, warnings);
        Assert.Equal(2, warnings.Count);
    }

    [Fact]
    public void ForEachWarning_ShouldNotInvokeAction_WhenNoWarnings()
    {
        var result = Result<string>.Ok("abc");
        bool called = false;
        result.ForEachWarning(w => called = true);
        Assert.False(called);
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrueAndValue_WhenPartialSuccess()
    {
        var warning = Warning.Create("W1", "warn");
        var result = Result<string>.PartialSuccess("abc", warning);
        var success = result.TryGetValue(out var value);
        Assert.True(success);
        Assert.Equal("abc", value);
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalseAndDefault_WhenControlledError()
    {
        var error = Error.Create("E", "fail");
        var warning = Warning.Create("W1", "warn");
        var result = Result<string>.ControlledError(error, warning);
        var success = result.TryGetValue(out var value);
        Assert.False(success);
        Assert.Null(value);
    }
}
