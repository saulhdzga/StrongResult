using StrongResult.Common;

namespace StrongResult.Generic;

/// <summary>
/// Provides Match extension methods for <see cref="Result{T}"/>.
/// </summary>
public static partial class ResultTExtensions
{
    /// <summary>
    /// Matches the result and executes the appropriate function based on success or failure.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="onFailure">The function to execute if the result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    public static TResult Match<T, TResult>(this Result<T> result, Func<T, TResult> onSuccess, Func<IError, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);
        return result.IsSuccess ? onSuccess(result.Value!) : onFailure(result.Error!);
    }

    /// <summary>
    /// Asynchronously matches the result and executes the appropriate function based on success or failure.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <typeparam name="TResult">The type of the return value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">The asynchronous function to execute if the result is successful.</param>
    /// <param name="onFailure">The asynchronous function to execute if the result is a failure.</param>
    /// <returns>A task representing the asynchronous operation, with the result of the executed function as the result.</returns>
    public static async ValueTask<TResult> MatchAsync<T, TResult>(this Result<T> result, Func<T, ValueTask<TResult>> onSuccess, Func<IError, ValueTask<TResult>> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);
        return result.IsSuccess ? await onSuccess(result.Value!).ConfigureAwait(false) : await onFailure(result.Error!).ConfigureAwait(false);
    }
}
