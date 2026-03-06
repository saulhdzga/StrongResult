using StrongResult.Common;

namespace StrongResult.NonGeneric;

/// <summary>
/// Provides Match extension methods for the non-generic <see cref="Result"/> type.
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Matches the result and executes the appropriate function based on success or failure.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="OnFailureure">The function to execute if the result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    public static T Match<T>(this Result result, Func<Result, T> onSuccess, Func<IError, T> OnFailureure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(OnFailureure);
        return result.IsSuccess ? onSuccess(result) : OnFailureure(result.Error!);
    }

    /// <summary>
    /// Asynchronously matches the result and executes the appropriate function based on success or failure.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">The asynchronous function to execute if the result is successful.</param>
    /// <param name="OnFailureure">The asynchronous function to execute if the result is a failure.</param>
    /// <returns>A task representing the asynchronous operation, with the result of the executed function as the result.</returns>
    public static async ValueTask<T> MatchAsync<T>(this Result result, Func<Result, ValueTask<T>> onSuccess, Func<IError, ValueTask<T>> OnFailureure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(OnFailureure);
        return result.IsSuccess ? await onSuccess(result).ConfigureAwait(false) : await OnFailureure(result.Error!).ConfigureAwait(false);
    }

    /// <summary>
    /// Matches an asynchronous result and executes the appropriate synchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="OnFailureure">The function to execute if the result is a failure.</param>
    /// <returns>A task representing the asynchronous operation, with the result of the executed function as the result.</returns>
    public static async ValueTask<T> MatchAsync<T>(this ValueTask<Result> resultTask, Func<Result, T> onSuccess, Func<IError, T> OnFailureure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(OnFailureure);
        var result = await resultTask.ConfigureAwait(false);
        return result.Match(onSuccess, OnFailureure);
    }

    /// <summary>
    /// Matches an asynchronous result and executes the appropriate asynchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="onSuccess">The asynchronous function to execute if the result is successful.</param>
    /// <param name="OnFailureure">The asynchronous function to execute if the result is a failure.</param>
    /// <returns>A task representing the asynchronous operation, with the result of the executed function as the result.</returns>
    public static async ValueTask<T> MatchAsync<T>(this ValueTask<Result> resultTask, Func<Result, ValueTask<T>> onSuccess, Func<IError, ValueTask<T>> OnFailureure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(OnFailureure);
        var result = await resultTask.ConfigureAwait(false);
        return await result.MatchAsync(onSuccess, OnFailureure).ConfigureAwait(false);
    }

    /// <summary>
    /// Matches an asynchronous result (Task) and executes the appropriate synchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="OnFailureure">The function to execute if the result is a failure.</param>
    /// <returns>A task representing the asynchronous operation, with the result of the executed function as the result.</returns>
    public static async Task<T> MatchAsync<T>(this Task<Result> resultTask, Func<Result, T> onSuccess, Func<IError, T> OnFailureure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(OnFailureure);
        var result = await resultTask.ConfigureAwait(false);
        return result.Match(onSuccess, OnFailureure);
    }

    /// <summary>
    /// Matches an asynchronous result (Task) and executes the appropriate asynchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="onSuccess">The asynchronous function to execute if the result is successful.</param>
    /// <param name="OnFailureure">The asynchronous function to execute if the result is a failure.</param>
    /// <returns>A task representing the asynchronous operation, with the result of the executed function as the result.</returns>
    public static async Task<T> MatchAsync<T>(this Task<Result> resultTask, Func<Result, ValueTask<T>> onSuccess, Func<IError, ValueTask<T>> OnFailureure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(OnFailureure);
        var result = await resultTask.ConfigureAwait(false);
        return await result.MatchAsync(onSuccess, OnFailureure).ConfigureAwait(false);
    }
}
