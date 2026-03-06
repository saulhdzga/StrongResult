using StrongResult.Common;
using StrongResult.Generic;

namespace StrongResult.Test.Generic;

public class ResultTMatchTests
{
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
    public void Match_ShouldThrowArgumentNullException_WhenOnSuccessIsNull()
    {
        var result = Result<string>.Ok("abc");
        Assert.Throws<ArgumentNullException>(() => result.Match<string, int>(null!, e => -1));
    }

    [Fact]
    public void Match_ShouldThrowArgumentNullException_WhenOnFailureIsNull()
    {
        var result = Result<string>.Ok("abc");
        Assert.Throws<ArgumentNullException>(() => result.Match(s => s.Length, null!));
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
    public async Task MatchAsync_ShouldThrowArgumentNullException_WhenOnSuccessIsNull()
    {
        var result = Result<string>.Ok("abc");
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.MatchAsync<string, int>(null!, async e => await Task.FromResult(-1)).AsTask());
    }

    [Fact]
    public async Task MatchAsync_ShouldThrowArgumentNullException_WhenOnFailureIsNull()
    {
        var result = Result<string>.Ok("abc");
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.MatchAsync(async s => await Task.FromResult(s.Length), null!).AsTask());
    }
}
