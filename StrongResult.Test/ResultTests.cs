using StrongResult.Common;
using StrongResult.NonGeneric;

namespace StrongResult.Test;

public class ResultTests
{
    [Fact]
    public void Ok_ShouldBeSuccess_AndHardSuccessKind()
    {
        var result = Result.Ok();
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(ResultKind.HardSuccess, result.Kind);
        Assert.Null(result.Error);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public void PartialSuccess_ShouldBeSuccess_AndPartialSuccessKind()
    {
        var warning = Warning.Create("W1", "Test warning");
        var result = Result.PartialSuccess(warning);
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultKind.PartialSuccess, result.Kind);
        Assert.Contains(warning, result.Warnings);
    }

    [Fact]
    public void PartialSuccess_ShouldAddUnknownWarning_WhenNoWarningsProvided()
    {
        var result = Result.PartialSuccess();
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultKind.PartialSuccess, result.Kind);
        Assert.Single(result.Warnings);
        Assert.IsType<UnknownWarning>(result.Warnings[0]);
    }

    [Fact]
    public void ControlledError_ShouldBeFailure_AndControlledErrorKind()
    {
        var error = Error.Create("E1", "Test error");
        var warning = Warning.Create("W1", "Test warning");
        var result = Result.ControlledError(error, warning);
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(ResultKind.ControlledError, result.Kind);
        Assert.Equal(error, result.Error);
        Assert.Contains(warning, result.Warnings);
    }

    [Fact]
    public void Fail_ShouldBeFailure_AndHardFailureKind()
    {
        var error = Error.Create("E1", "Test error");
        var result = Result.Fail(error);
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultKind.HardFailure, result.Kind);
        Assert.Equal(error, result.Error);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public void Fail_ShouldThrowArgumentNullException_WhenErrorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Fail((Error)null!));
    }

    [Fact]
    public void Fail_ShouldThrowArgumentNullException_WhenExceptionIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Fail((Exception)null!));
    }

    [Fact]
    public void ControlledError_ShouldThrowArgumentNullException_WhenErrorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.ControlledError((Error)null!));
    }

    [Fact]
    public void ControlledError_ShouldBeFailure_AndControlledErrorKind_FromException()
    {
        var ex = new InvalidOperationException("Test exception");
        var warning = Warning.Create("W1", "Test warning");
        var result = Result.ControlledError(ex, warning);
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(ResultKind.ControlledError, result.Kind);
        Assert.NotNull(result.Error);
        Assert.Contains(warning, result.Warnings);
        Assert.Contains("Test exception", result.Error!.Message);
    }

    [Fact]
    public void ControlledError_ShouldThrowArgumentNullException_WhenExceptionIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result.ControlledError((Exception)null!));
    }

    [Fact]
    public void ControlledError_ShouldAddUnknownWarning_WhenNoWarningsProvided()
    {
        var error = Error.Create("E1", "Test error");
        var result = Result.ControlledError(error);
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultKind.ControlledError, result.Kind);
        Assert.Single(result.Warnings);
        Assert.IsType<UnknownWarning>(result.Warnings[0]);
    }
}
