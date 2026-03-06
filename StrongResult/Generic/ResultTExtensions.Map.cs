using StrongResult.Common;

namespace StrongResult.Generic;

/// <summary>
/// Provides Map extension methods for <see cref="Result{T}"/>.
/// </summary>
public static partial class ResultTExtensions
{
    /// <summary>
    /// Maps the value of a successful result to a new value using the specified function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the mapped value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="func">The mapping function.</param>
    /// <returns>A new <see cref="Result{U}"/> with the mapped value, or a failure result if this result is a failure.</returns>
    public static Result<U> Map<T, U>(this Result<T> result, Func<T, U> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        if (result.IsFailure)
        {
            return result.Error != null
                ? result.Warnings.Count != 0 ? Result<U>.ControlledError(result.Error, [.. result.Warnings]) : Result<U>.Fail(result.Error)
                : Result<U>.Fail(result.Error!);
        }
        return result.Warnings.Count != 0
            ? Result<U>.PartialSuccess(func(result.Value!), result.Warnings.ToArray())
            : Result<U>.Ok(func(result.Value!));
    }

    /// <summary>
    /// Asynchronously maps the value of a successful result to a new value using the specified asynchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the mapped value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="func">The asynchronous mapping function.</param>
    /// <returns>A task representing the asynchronous operation, with a new <see cref="Result{U}"/> as the result.</returns>
    public static async ValueTask<Result<U>> MapAsync<T, U>(this Result<T> result, Func<T, ValueTask<U>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        if (result.IsFailure)
        {
            return result.Error != null
                ? result.Warnings.Count != 0 ? Result<U>.ControlledError(result.Error, result.Warnings.ToArray()) : Result<U>.Fail(result.Error)
                : Result<U>.Fail(result.Error!);
        }
        var mapped = await func(result.Value!).ConfigureAwait(false);
        return result.Warnings.Count != 0
            ? Result<U>.PartialSuccess(mapped, result.Warnings.ToArray())
            : Result<U>.Ok(mapped);
    }

    /// <summary>
    /// Maps an asynchronous result to a new value using the specified synchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the mapped value.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="func">The mapping function.</param>
    /// <returns>A task representing the asynchronous operation, with a new <see cref="Result{U}"/> as the result.</returns>
    public static async ValueTask<Result<U>> MapAsync<T, U>(this ValueTask<Result<T>> resultTask, Func<T, U> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        var result = await resultTask.ConfigureAwait(false);
        return result.Map(func);
    }

    /// <summary>
    /// Maps an asynchronous result to a new value using the specified asynchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the mapped value.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="func">The asynchronous mapping function.</param>
    /// <returns>A task representing the asynchronous operation, with a new <see cref="Result{U}"/> as the result.</returns>
    public static async ValueTask<Result<U>> MapAsync<T, U>(this ValueTask<Result<T>> resultTask, Func<T, ValueTask<U>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        var result = await resultTask.ConfigureAwait(false);
        return await result.MapAsync(func).ConfigureAwait(false);
    }

    /// <summary>
    /// Maps an asynchronous result (Task) to a new value using the specified synchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the mapped value.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="func">The mapping function.</param>
    /// <returns>A task representing the asynchronous operation, with a new <see cref="Result{U}"/> as the result.</returns>
    public static async Task<Result<U>> MapAsync<T, U>(this Task<Result<T>> resultTask, Func<T, U> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        var result = await resultTask.ConfigureAwait(false);
        return result.Map(func);
    }

    /// <summary>
    /// Maps an asynchronous result (Task) to a new value using the specified asynchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the mapped value.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="func">The asynchronous mapping function.</param>
    /// <returns>A task representing the asynchronous operation, with a new <see cref="Result{U}"/> as the result.</returns>
    public static async Task<Result<U>> MapAsync<T, U>(this Task<Result<T>> resultTask, Func<T, ValueTask<U>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        var result = await resultTask.ConfigureAwait(false);
        return await result.MapAsync(func).ConfigureAwait(false);
    }
}
