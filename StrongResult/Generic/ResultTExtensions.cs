using StrongResult.Common;
using System.Diagnostics.CodeAnalysis;

namespace StrongResult.Generic;

/// <summary>
/// Provides functional-style extension methods for <see cref="Result{T}"/>.
/// </summary>
public static class ResultTExtensions
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

    /// <summary>
    /// Returns a string representation of the result, including success, kind, error, and warnings.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <returns>A string representation of the result.</returns>
    public static string ToString<T>(this Result<T> result)
    {
        return $"Result {{ IsSuccess = {result.IsSuccess}, IsFailure = {result.IsFailure}, Kind = {result.Kind}, Value = {result.Value}, Error = {result.Error?.Message ?? "None"}, Warnings = [{string.Join(", ", result.Warnings.Select(w => w.Message))}] }}";
    }

    /// <summary>
    /// Attempts to get the value if the result is successful.
    /// </summary>
    /// <typeparam name="T">The type of the value in the result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="value">When this method returns, contains the value if the result is successful; otherwise, the default value for the type.</param>
    /// <returns><c>true</c> if the result is successful; otherwise, <c>false</c>.</returns>
    public static bool TryGetValue<T>(this Result<T> result, [NotNullWhen(true)] out T? value)
    {
        if (result.IsSuccess)
        {
            value = result.Value;
            return true;
        }
        value = default;
        return false;
    }
}
