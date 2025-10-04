using StrongResult.Common;

namespace StrongResult.NonGeneric;

/// <summary>
/// Provides functional-style extension methods for the non-generic <see cref="Result"/> type.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Executes the specified action if the result is successful.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute on success.</param>
    /// <returns>The current <see cref="Result"/> instance.</returns>
    public static Result OnSuccess(this Result result, Action action)
    {
        if (result.IsSuccess) action();
        return result;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the result is successful.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The asynchronous action to execute on success.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result"/> instance as the result.</returns>
    public static async Task<Result> OnSuccessAsync(this Result result, Func<Task> action)
    {
        if (result.IsSuccess) await action();
        return result;
    }

    /// <summary>
    /// Executes the specified action if the result is a failure.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute on error.</param>
    /// <returns>The current <see cref="Result"/> instance.</returns>
    public static Result OnFailure(this Result result, Action<IError> action)
    {
        if (result.IsFailure) action(result.Error!);
        return result;
    }

    /// <summary>
    /// Asynchronously executes the specified action if the result is a failure.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The asynchronous action to execute on error.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result"/> instance as the result.</returns>
    public static async Task<Result> OnFailureAsync(this Result result, Func<IError, Task> action)
    {
        if (result.IsFailure) await action(result.Error!);
        return result;
    }

    /// <summary>
    /// Executes the specified action if there are any warnings.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute on warnings.</param>
    /// <returns>The current <see cref="Result"/> instance.</returns>
    public static Result OnWarnings(this Result result, Action<IReadOnlyList<IWarning>> action)
    {
        if (result.Warnings.Count != 0) action(result.Warnings);
        return result;
    }

    /// <summary>
    /// Asynchronously executes the specified action if there are any warnings.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The asynchronous action to execute on warnings.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result"/> instance as the result.</returns>
    public static async Task<Result> OnWarningsAsync(this Result result, Func<IReadOnlyList<IWarning>, Task> action)
    {
        if (result.Warnings.Count != 0) await action(result.Warnings);
        return result;
    }

    /// <summary>
    /// Executes the specified action for each warning.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute for each warning.</param>
    /// <returns>The current <see cref="Result"/> instance.</returns>
    public static Result ForEachWarning(this Result result, Action<IWarning> action)
    {
        foreach (var w in result.Warnings) action(w);
        return result;
    }

    /// <summary>
    /// Asynchronously executes the specified action for each warning.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The asynchronous action to execute for each warning.</param>
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result"/> instance as the result.</returns>
    public static async Task<Result> ForEachWarningAsync(this Result result, Func<IWarning, Task> action)
    {
        foreach (var w in result.Warnings) await action(w);
        return result;
    }

    /// <summary>
    /// Matches the result and executes the appropriate function based on success or failure.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">The function to execute if the result is successful.</param>
    /// <param name="OnFailureure">The function to execute if the result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    public static T Match<T>(this Result result, Func<T> onSuccess, Func<IError, T> OnFailureure)
    {
        return result.IsSuccess ? onSuccess() : OnFailureure(result.Error!);
    }

    /// <summary>
    /// Asynchronously matches the result and executes the appropriate function based on success or failure.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="result">The source result.</param>
    /// <param name="onSuccess">The asynchronous function to execute if the result is successful.</param>
    /// <param name="OnFailureure">The asynchronous function to execute if the result is a failure.</param>
    /// <returns>A task representing the asynchronous operation, with the result of the executed function as the result.</returns>
    public static async Task<T> MatchAsync<T>(this Result result, Func<Task<T>> onSuccess, Func<IError, Task<T>> OnFailureure)
    {
        return result.IsSuccess ? await onSuccess() : await OnFailureure(result.Error!);
    }

    /// <summary>
    /// Returns a string representation of the result, including success, kind, error, and warnings.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <returns>A string representation of the result.</returns>
    public static string ToString(this Result result)
    {
        return $"Result {{ IsSuccess = {result.IsSuccess}, IsFailure = {result.IsFailure}, Kind = {result.Kind}, Error = {result.Error?.Message ?? "None"}, Warnings = [{string.Join(", ", result.Warnings.Select(w => w.Message))}] }}";
    }
}
