using StrongResult.Common;

namespace StrongResult.NonGeneric;

/// <summary>
/// Provides OnSuccess extension methods for the non-generic <see cref="Result"/> type.
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Executes the specified action if the result is successful.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute on success.</param>
    /// <returns>The current <see cref="Result"/> instance.</returns>
    public static Result OnSuccess(this Result result, Action<Result> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (result.IsSuccess) action(result);
        return result;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the result is successful.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The asynchronous action to execute on success.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result"/> instance as the result.</returns>
    public static async ValueTask<Result> OnSuccessAsync(this Result result, Func<Result, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (result.IsSuccess) await action(result).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes the specified synchronous action on an asynchronous result if it is successful.
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The action to execute on success.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result"/> instance as the result.</returns>
    public static async ValueTask<Result> OnSuccessAsync(this ValueTask<Result> resultTask, Action<Result> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return result.OnSuccess(action);
    }

    /// <summary>
    /// Executes the specified asynchronous action on an asynchronous result if it is successful.
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The asynchronous action to execute on success.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result"/> instance as the result.</returns>
    public static async ValueTask<Result> OnSuccessAsync(this ValueTask<Result> resultTask, Func<Result, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return await result.OnSuccessAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the specified synchronous action on an asynchronous result (Task) if it is successful.
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The action to execute on success.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result"/> instance as the result.</returns>
    public static async Task<Result> OnSuccessAsync(this Task<Result> resultTask, Action<Result> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return result.OnSuccess(action);
    }

    /// <summary>
    /// Executes the specified asynchronous action on an asynchronous result (Task) if it is successful.
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The asynchronous action to execute on success.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result"/> instance as the result.</returns>
    public static async Task<Result> OnSuccessAsync(this Task<Result> resultTask, Func<Result, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return await result.OnSuccessAsync(action).ConfigureAwait(false);
    }
}
