using StrongResult.Common;

namespace StrongResult.NonGeneric;

/// <summary>
/// Provides OnFailure extension methods for the non-generic <see cref="Result"/> type.
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Executes the specified action if the result is a failure.
    /// </summary>
    /// <param name="result">The source result.</param>
    /// <param name="action">The action to execute on error.</param>
    /// <returns>The current <see cref="Result"/> instance.</returns>
    public static Result OnFailure(this Result result, Action<IError> action)
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
    /// <returns>A task representing the asynchronous operation, with the current <see cref="Result"/> instance as the result.</returns>
    public static async ValueTask<Result> OnFailureAsync(this Result result, Func<IError, ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (result.IsFailure) await action(result.Error!).ConfigureAwait(false);
        return result;
    }
}
