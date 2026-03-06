using StrongResult.Common;

namespace StrongResult.Generic;

/// <summary>
/// Provides OnFailure extension methods for <see cref="Result{T}"/>.
/// </summary>
public static partial class ResultTExtensions
{
    /// <summary>
    /// Executes the specified action if the result is a failure.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute on error.</param>
    /// <returns>The current <see cref="Result{T}"/> instance.</returns>
    public static Result<T> OnFailure<T>(this Result<T> result, Action<IError> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (result.IsFailure) action(result.Error!);
        return result;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the result is a failure.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The asynchronous action to execute on error.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result{T}"/> instance as the result.</returns>
    public static async ValueTask<Result<T>> OnFailureAsync<T>(this Result<T> result, Func<IError, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (result.IsFailure) await action(result.Error!).ConfigureAwait(false);
        return result;
    }
}
