namespace StrongResult.Test.Integration;

using StrongResult.Common;
using StrongResult.Generic;
using NonGenericResult = StrongResult.NonGeneric.Result;
using StrongResult.NonGeneric;

public class AsyncChainingIntegrationTests
{
    [Fact]
    public async Task AsyncChaining_MapBindOnSuccess_ShouldChainCorrectly()
    {
        var resultTask = Task.FromResult(Result<int>.Ok(5));

        var output = await resultTask
            .MapAsync(x => x * 2)
            .BindAsync(async x => await ValueTask.FromResult(Result<string>.Ok(x.ToString())))
            .OnSuccessAsync(async s =>
            {
                await Task.Yield();
                Assert.Equal("10", s);
            });

        Assert.True(output.IsSuccess);
        Assert.Equal("10", output.Value);
    }

    [Fact]
    public async Task AsyncChaining_WithFailure_ShouldShortCircuit()
    {
        var error = Error.Create("E", "error");
        var resultTask = Task.FromResult(Result<int>.Fail(error));
        var mapCalled = false;
        var successCalled = false;
        var failureCalled = false;

        var output = await resultTask
            .MapAsync(x =>
            {
                mapCalled = true;
                return x * 2;
            })
            .OnSuccessAsync(s => successCalled = true)
            .OnFailureAsync(e => failureCalled = true);

        Assert.False(mapCalled);
        Assert.False(successCalled);
        Assert.True(failureCalled);
        Assert.False(output.IsSuccess);
    }

    [Fact]
    public async Task AsyncChaining_WithWarnings_ShouldPreserveWarnings()
    {
        var warning = Warning.Create("W", "warning");
        var resultTask = Task.FromResult(Result<int>.PartialSuccess(5, warning));
        var warningCount = 0;

        var output = await resultTask
            .MapAsync(x => x * 2)
            .ForEachWarningAsync(async w =>
            {
                await Task.Yield();
                warningCount++;
            });

        Assert.True(output.IsSuccess);
        Assert.Equal(10, output.Value);
        Assert.Single(output.Warnings);
        Assert.Equal(1, warningCount);
    }

    [Fact]
    public async Task AsyncChaining_ComplexPipeline_ShouldHandleAllStates()
    {
        var warning1 = Warning.Create("W1", "input warning");
        var warning2 = Warning.Create("W2", "processing warning");
        
        var resultTask = Task.FromResult(Result<string>.PartialSuccess("123", warning1));
        
        var warningsSeen = new List<IWarning>();
        var successExecuted = false;

        var output = await resultTask
            .MapAsync(async s => await ValueTask.FromResult(int.Parse(s)))
            .BindAsync(async i => await ValueTask.FromResult(
                i > 0 ? Result<double>.PartialSuccess(i * 1.5, warning2) : Result<double>.Fail(Error.Create("E", "negative"))))
            .OnWarningsAsync(async w =>
            {
                await Task.Yield();
                warningsSeen.AddRange(w);
            })
            .OnSuccessAsync(async d =>
            {
                await Task.Yield();
                successExecuted = true;
            });

        Assert.True(output.IsSuccess);
        Assert.Equal(184.5, output.Value);
        Assert.Equal(2, output.Warnings.Count);
        Assert.Equal(2, warningsSeen.Count);
        Assert.True(successExecuted);
    }

    [Fact]
    public async Task AsyncChaining_MatchAtEnd_ShouldWorkCorrectly()
    {
        var resultTask = Task.FromResult(Result<int>.Ok(42));

        var output = await resultTask
            .MapAsync(async x => await ValueTask.FromResult(x.ToString()))
            .MatchAsync(
                async s => await ValueTask.FromResult($"Success: {s}"),
                async e => await ValueTask.FromResult($"Error: {e.Message}"));

        Assert.Equal("Success: 42", output);
    }

    [Fact]
    public async Task AsyncChaining_NonGeneric_ShouldWorkCorrectly()
    {
        var successCalled = false;
        var warningCalled = false;

        var warning = Warning.Create("W", "warning");
        var resultTask = Task.FromResult(NonGenericResult.PartialSuccess(warning));

        var output = await resultTask
            .OnSuccessAsync(async r =>
            {
                await Task.Yield();
                successCalled = true;
            })
            .OnWarningsAsync(async w =>
            {
                await Task.Yield();
                warningCalled = true;
            });

        Assert.True(successCalled);
        Assert.True(warningCalled);
        Assert.True(output.IsSuccess);
        Assert.Single(output.Warnings);
    }
}
