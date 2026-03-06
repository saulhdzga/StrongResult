using StrongResult.Common;
using StrongResult.NonGeneric;

namespace StrongResult.Test.NonGeneric;

public class ResultMatchTests
{
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

    [Fact]
    public async Task MatchAsync_ShouldThrowArgumentNullException_WhenOnSuccessIsNull()
    {
        var result = Result.Ok();
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.MatchAsync<string>(null!, async e => await Task.FromResult("failure")).AsTask());
    }

    [Fact]
    public async Task MatchAsync_ShouldThrowArgumentNullException_WhenOnFailureIsNull()
    {
        var result = Result.Ok();
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.MatchAsync(async r => await Task.FromResult("success"), null!).AsTask());
    }

    [Fact]
    public async Task MatchAsync_ValueTaskSource_WithSyncFuncs_ShouldMatchSuccess()
    {
        var resultTask = new ValueTask<Result>(Result.Ok());
        var output = await resultTask.MatchAsync(r => "success", e => "failure");
        Assert.Equal("success", output);
    }

    [Fact]
    public async Task MatchAsync_ValueTaskSource_WithAsyncFuncs_ShouldMatchSuccess()
    {
        var resultTask = new ValueTask<Result>(Result.Ok());
        var output = await resultTask.MatchAsync(
            async r => await ValueTask.FromResult("success"),
            async e => await ValueTask.FromResult("failure"));
        Assert.Equal("success", output);
    }

    [Fact]
    public async Task MatchAsync_TaskSource_WithSyncFuncs_ShouldMatchFailure()
    {
        var error = Error.Create("E", "error");
        var resultTask = Task.FromResult(Result.Fail(error));
        var output = await resultTask.MatchAsync(r => "success", e => "failure");
        Assert.Equal("failure", output);
    }

    [Fact]
    public async Task MatchAsync_TaskSource_WithAsyncFuncs_ShouldMatchFailure()
    {
        var error = Error.Create("E", "error");
        var resultTask = Task.FromResult(Result.Fail(error));
        var output = await resultTask.MatchAsync(
            async r => await ValueTask.FromResult("success"),
            async e => await ValueTask.FromResult("failure"));
        Assert.Equal("failure", output);
    }
}
