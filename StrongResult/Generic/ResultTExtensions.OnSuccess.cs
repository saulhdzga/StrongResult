using StrongResult.Common;

namespace StrongResult.Generic;

/// <summary>
/// Provides OnSuccess extension methods for <see cref="Result{T}"/>.
/// </summary>
public static partial class ResultTExtensions
{
    /// <summary>
    /// Executes the specified action if the result is successful.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute on success.</param>
    /// <returns>The current <see cref="Result{T}"/> instance.</returns>
    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (result.IsSuccess) action(result.Value!);
        return result;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the result is successful.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The asynchronous action to execute on success.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async ValueTask<Result<T>> OnSuccessAsync<T>(this Result<T> result, Func<T, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (result.IsSuccess) await action(result.Value!).ConfigureAwait(false);
        return result;
    }
}
