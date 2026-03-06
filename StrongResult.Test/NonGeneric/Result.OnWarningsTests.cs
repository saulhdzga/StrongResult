using StrongResult.Common;
using StrongResult.NonGeneric;

namespace StrongResult.Test.NonGeneric;

public class ResultOnWarningsTests
{
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
    public async Task OnWarningsAsync_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        var result = Result.PartialSuccess(Warning.Create("W", "warn"));
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.OnWarningsAsync(null!).AsTask());
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
    public async Task ForEachWarningAsync_ShouldThrowArgumentNullException_WhenActionIsNull()
    {
        var result = Result.PartialSuccess(Warning.Create("W", "warn"));
        await Assert.ThrowsAsync<ArgumentNullException>(() => result.ForEachWarningAsync(null!).AsTask());
    }

    [Fact]
    public async Task OnWarningsAsync_ValueTaskSource_WithSyncAction_ShouldExecuteOnWarnings()
    {
        var executed = false;
        var warning = Warning.Create("W", "warning");
        var resultTask = new ValueTask<Result>(Result.PartialSuccess(warning));
        var result = await resultTask.OnWarningsAsync(w => executed = true);
        Assert.True(executed);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task OnWarningsAsync_ValueTaskSource_WithAsyncAction_ShouldExecuteOnWarnings()
    {
        var executed = false;
        var warning = Warning.Create("W", "warning");
        var resultTask = new ValueTask<Result>(Result.PartialSuccess(warning));
        var result = await resultTask.OnWarningsAsync(async w =>
        {
            await Task.Yield();
            executed = true;
        });
        Assert.True(executed);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ForEachWarningAsync_ValueTaskSource_WithSyncAction_ShouldIterateWarnings()
    {
        var count = 0;
        var warning1 = Warning.Create("W1", "warning1");
        var warning2 = Warning.Create("W2", "warning2");
        var resultTask = new ValueTask<Result>(Result.PartialSuccess(warning1, warning2));
        var result = await resultTask.ForEachWarningAsync(w => count++);
        Assert.Equal(2, count);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ForEachWarningAsync_ValueTaskSource_WithAsyncAction_ShouldIterateWarnings()
    {
        var count = 0;
        var warning1 = Warning.Create("W1", "warning1");
        var warning2 = Warning.Create("W2", "warning2");
        var resultTask = new ValueTask<Result>(Result.PartialSuccess(warning1, warning2));
        var result = await resultTask.ForEachWarningAsync(async w =>
        {
            await Task.Yield();
            count++;
        });
        Assert.Equal(2, count);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ForEachWarningAsync_TaskSource_WithSyncAction_ShouldIterateWarnings()
    {
        var count = 0;
        var warning1 = Warning.Create("W1", "warning1");
        var warning2 = Warning.Create("W2", "warning2");
        var resultTask = Task.FromResult(Result.PartialSuccess(warning1, warning2));
        var result = await resultTask.ForEachWarningAsync(w => count++);
        Assert.Equal(2, count);
        Assert.True(result.IsSuccess);
    }
}
