using StrongResult.Common;
using StrongResult.Generic;
using Xunit;

namespace StrongResult.Test;

public class ResultTTests
{
    [Fact]
    public void Ok_ShouldBeSuccess_AndHardSuccessKind_AndReturnValue()
    {
        var result = Result<string>.Ok("value");
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(ResultKind.HardSuccess, result.Kind);
        Assert.Null(result.Error);
        Assert.Empty(result.Warnings);
        Assert.Equal("value", result.Value);
    }

    [Fact]
    public void PartialSuccess_ShouldBeSuccess_AndPartialSuccessKind_AndReturnValue()
    {
        var warning = Warning.Create("W1", "Test warning");
        var result = Result<string>.PartialSuccess("value", warning);
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultKind.PartialSuccess, result.Kind);
        Assert.Contains(warning, result.Warnings);
        Assert.Equal("value", result.Value);
    }

    [Fact]
    public void ControlledError_ShouldBeFailure_AndControlledErrorKind()
    {
        var error = Error.Create("E1", "Test error");
        var warning = Warning.Create("W1", "Test warning");
        var result = Result<string>.ControlledError(error, warning);
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(ResultKind.ControlledError, result.Kind);
        Assert.Equal(error, result.Error);
        Assert.Contains(warning, result.Warnings);
        Assert.Null(result.Value);
    }

    [Fact]
    public void Fail_ShouldBeFailure_AndHardFailureKind()
    {
        var error = Error.Create("E1", "Test error");
        var result = Result<string>.Fail(error);
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultKind.HardFailure, result.Kind);
        Assert.Equal(error, result.Error);
        Assert.Empty(result.Warnings);
        Assert.Null(result.Value);
    }

    [Fact]
    public void Fail_ShouldThrowArgumentNullException_WhenErrorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result<string>.Fail((Error)null!));
    }

    [Fact]
    public void Fail_ShouldThrowArgumentNullException_WhenExceptionIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result<string>.Fail((Exception)null!));
    }

    [Fact]
    public void ControlledError_ShouldThrowArgumentNullException_WhenErrorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result<string>.ControlledError((Error)null!));
    }

    [Fact]
    public void ControlledError_ShouldBeFailure_AndControlledErrorKind_FromException()
    {
        var ex = new InvalidOperationException("Test exception");
        var warning = Warning.Create("W1", "Test warning");
        var result = Result<string>.ControlledError(ex, warning);
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(ResultKind.ControlledError, result.Kind);
        Assert.NotNull(result.Error);
        Assert.Contains(warning, result.Warnings);
        Assert.Null(result.Value);
        Assert.Contains("Test exception", result.Error!.Message);
    }

    [Fact]
    public void ControlledError_ShouldThrowArgumentNullException_WhenExceptionIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => Result<string>.ControlledError((Exception)null!));
    }

    [Fact]
    public void PartialSuccess_ShouldAddUnknownWarning_WhenNoWarningsProvided()
    {
        var result = Result<string>.PartialSuccess("value");
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultKind.PartialSuccess, result.Kind);
        Assert.Single(result.Warnings);
        Assert.IsType<UnknownWarning>(result.Warnings[0]);
        Assert.Equal("value", result.Value);
    }

    [Fact]
    public void ControlledError_ShouldAddUnknownWarning_WhenNoWarningsProvided()
    {
        var error = Error.Create("E1", "Test error");
        var result = Result<string>.ControlledError(error);
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultKind.ControlledError, result.Kind);
        Assert.Single(result.Warnings);
        Assert.IsType<UnknownWarning>(result.Warnings[0]);
        Assert.Null(result.Value);
    }
}
