using StrongResult.Common;

namespace StrongResult.Generic;

/// <summary>
/// Provides Bind extension methods for <see cref="Result{T}"/>.
/// </summary>
public static partial class ResultTExtensions
{
    /// <summary>
    /// Binds the value of a successful result to a new result using the specified function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the value in the new result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="func">The binding function.</param>
    /// <returns>A new <see cref="Result{U}"/> returned by the binding function, or a failure result if this result is a failure.</returns>
    public static Result<U> Bind<T, U>(this Result<T> result, Func<T, Result<U>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        if (result.IsFailure)
        {
            return result.Error != null
                ? result.Warnings.Count != 0 ? Result<U>.ControlledError(result.Error, [.. result.Warnings]) : Result<U>.Fail(result.Error)
                : Result<U>.Fail(result.Error!);
        }
        var next = func(result.Value!);
        var combinedWarnings = result.Warnings.Concat(next.Warnings).ToList();
        if (next.IsFailure)
        {
            return next.Error != null
                ? combinedWarnings.Count != 0 ? Result<U>.ControlledError(next.Error, [.. combinedWarnings]) : Result<U>.Fail(next.Error)
                : Result<U>.Fail(next.Error!);
        }
        return combinedWarnings.Count != 0
            ? Result<U>.PartialSuccess(next.Value!, [.. combinedWarnings])
            : Result<U>.Ok(next.Value!);
    }

    /// <summary>
    /// Asynchronously binds the value of a successful result to a new result using the specified asynchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the value in the new result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="func">The asynchronous binding function.</param>
    /// <returns>A task representing the asynchronous operation, with a new <see cref="Result{U}"/> as the result.</returns>
    public static async ValueTask<Result<U>> BindAsync<T, U>(this Result<T> result, Func<T, ValueTask<Result<U>>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        if (result.IsFailure)
        {
            return result.Error != null
                ? result.Warnings.Count != 0 ? Result<U>.ControlledError(result.Error, [.. result.Warnings]) : Result<U>.Fail(result.Error)
                : Result<U>.Fail(result.Error!);
        }
        var next = await func(result.Value!).ConfigureAwait(false);
        var combinedWarnings = result.Warnings.Concat(next.Warnings).ToList();
        if (next.IsFailure)
        {
            return next.Error != null
                ? combinedWarnings.Count != 0 ? Result<U>.ControlledError(next.Error, [.. combinedWarnings]) : Result<U>.Fail(next.Error)
                : Result<U>.Fail(next.Error!);
        }
        return combinedWarnings.Count != 0
            ? Result<U>.PartialSuccess(next.Value!, [.. combinedWarnings])
            : Result<U>.Ok(next.Value!);
    }

    /// <summary>
    /// Binds an asynchronous result to a new result using the specified synchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the value in the new result.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="func">The binding function.</param>
    /// <returns>A task representing the asynchronous operation, with a new <see cref="Result{U}"/> as the result.</returns>
    public static async ValueTask<Result<U>> BindAsync<T, U>(this ValueTask<Result<T>> resultTask, Func<T, Result<U>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        var result = await resultTask.ConfigureAwait(false);
        return result.Bind(func);
    }

    /// <summary>
    /// Binds an asynchronous result to a new result using the specified asynchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the value in the new result.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="func">The asynchronous binding function.</param>
    /// <returns>A task representing the asynchronous operation, with a new <see cref="Result{U}"/> as the result.</returns>
    public static async ValueTask<Result<U>> BindAsync<T, U>(this ValueTask<Result<T>> resultTask, Func<T, ValueTask<Result<U>>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        var result = await resultTask.ConfigureAwait(false);
        return await result.BindAsync(func).ConfigureAwait(false);
    }

    /// <summary>
    /// Binds an asynchronous result (Task) to a new result using the specified synchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the value in the new result.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="func">The binding function.</param>
    /// <returns>A task representing the asynchronous operation, with a new <see cref="Result{U}"/> as the result.</returns>
    public static async Task<Result<U>> BindAsync<T, U>(this Task<Result<T>> resultTask, Func<T, Result<U>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        var result = await resultTask.ConfigureAwait(false);
        return result.Bind(func);
    }

    /// <summary>
    /// Binds an asynchronous result (Task) to a new result using the specified asynchronous function.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source result.</typeparam>
    /// <typeparam name="U">The type of the value in the new result.</typeparam>
    /// <param name="resultTask">The asynchronous source result.</param>
    /// <param name="func">The asynchronous binding function.</param>
    /// <returns>A task representing the asynchronous operation, with a new <see cref="Result{U}"/> as the result.</returns>
    public static async Task<Result<U>> BindAsync<T, U>(this Task<Result<T>> resultTask, Func<T, ValueTask<Result<U>>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        var result = await resultTask.ConfigureAwait(false);
        return await result.BindAsync(func).ConfigureAwait(false);
    }
}
