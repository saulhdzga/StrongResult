using StrongResult.Common;

namespace StrongResult.Generic;

/// <summary>
/// Provides OnWarnings and ForEachWarning extension methods for <see cref="Result{T}"/>.
/// </summary>
public static partial class ResultTExtensions
{
    /// <summary>
    /// Executes the specified action if there are any warnings.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute on warnings.</param>
    /// <returns>The current <see cref="Result{T}"/> instance.</returns>
    public static Result<T> OnWarnings<T>(this Result<T> result, Action<IReadOnlyList<IWarning>> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (result.Warnings.Count != 0) action(result.Warnings);
        return result;
    }

    /// <summary>
    /// Asynchronously executes the specified action if there are any warnings.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The asynchronous action to execute on warnings.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async ValueTask<Result<T>> OnWarningsAsync<T>(this Result<T> result, Func<IReadOnlyList<IWarning>, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (result.Warnings.Count != 0) await action(result.Warnings).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes the specified synchronous action on an asynchronous result if there are any warnings.
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The action to execute on warnings.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async ValueTask<Result<T>> OnWarningsAsync<T>(this ValueTask<Result<T>> resultTask, Action<IReadOnlyList<IWarning>> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return result.OnWarnings(action);
    }

    /// <summary>
    /// Executes the specified asynchronous action on an asynchronous result if there are any warnings.
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The asynchronous action to execute on warnings.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async ValueTask<Result<T>> OnWarningsAsync<T>(this ValueTask<Result<T>> resultTask, Func<IReadOnlyList<IWarning>, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return await result.OnWarningsAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the specified synchronous action on an asynchronous result (Task) if there are any warnings.
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The action to execute on warnings.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async Task<Result<T>> OnWarningsAsync<T>(this Task<Result<T>> resultTask, Action<IReadOnlyList<IWarning>> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return result.OnWarnings(action);
    }

    /// <summary>
    /// Executes the specified asynchronous action on an asynchronous result (Task) if there are any warnings.
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The asynchronous action to execute on warnings.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async Task<Result<T>> OnWarningsAsync<T>(this Task<Result<T>> resultTask, Func<IReadOnlyList<IWarning>, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return await result.OnWarningsAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the specified action for each warning.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute for each warning.</param>
    /// <returns>The current <see cref="Result{T}"/> instance.</returns>
    public static Result<T> ForEachWarning<T>(this Result<T> result, Action<IWarning> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        foreach (var w in result.Warnings) action(w);
        return result;
    }

    /// <summary>
    /// Asynchronously executes the specified action for each warning.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The asynchronous action to execute for each warning.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async ValueTask<Result<T>> ForEachWarningAsync<T>(this Result<T> result, Func<IWarning, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        foreach (var w in result.Warnings) await action(w).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Executes the specified synchronous action for each warning on an asynchronous result.
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The action to execute for each warning.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async ValueTask<Result<T>> ForEachWarningAsync<T>(this ValueTask<Result<T>> resultTask, Action<IWarning> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return result.ForEachWarning(action);
    }

    /// <summary>
    /// Executes the specified asynchronous action for each warning on an asynchronous result.
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The asynchronous action to execute for each warning.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async ValueTask<Result<T>> ForEachWarningAsync<T>(this ValueTask<Result<T>> resultTask, Func<IWarning, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return await result.ForEachWarningAsync(action).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the specified synchronous action for each warning on an asynchronous result (Task).
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The action to execute for each warning.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async Task<Result<T>> ForEachWarningAsync<T>(this Task<Result<T>> resultTask, Action<IWarning> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return result.ForEachWarning(action);
    }

    /// <summary>
    /// Executes the specified asynchronous action for each warning on an asynchronous result (Task).
    /// </summary>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="action">The asynchronous action to execute for each warning.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async Task<Result<T>> ForEachWarningAsync<T>(this Task<Result<T>> resultTask, Func<IWarning, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        var result = await resultTask.ConfigureAwait(false);
        return await result.ForEachWarningAsync(action).ConfigureAwait(false);
    }
}
