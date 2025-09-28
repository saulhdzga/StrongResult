namespace StrongResult.Common;

/// <summary>
/// Provides internal helper methods for result construction and validation.
/// </summary>
internal static class ResultHelpers
{
    /// <summary>
    /// Throws <see cref="ArgumentNullException"/> if the argument is null.
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="argument">The argument to check.</param>
    /// <param name="paramName">The name of the parameter.</param>
    public static void ThrowIfArgumentNull<T>(T? argument, string paramName)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");
        }
    }

    /// <summary>
    /// Throws <see cref="ArgumentException"/> if <paramref name="isSuccess"/> is true and <paramref name="error"/> is not null.
    /// </summary>
    /// <param name="isSuccess">Indicates if the result is a success.</param>
    /// <param name="error">The error to check.</param>
    public static void ThrowIfSuccessWithError(bool isSuccess, IError? error)
    {
        if (isSuccess && error is not null)
        {
            throw new ArgumentException("A successful result cannot have an error.", nameof(error));
        }
    }

    /// <summary>
    /// Ensures that the warnings array is not null or empty. If it is, returns an array with UnknownWarning.Instance.
    /// </summary>
    /// <param name="warnings">The warnings to check.</param>
    /// <returns>An array of warnings, guaranteed to have at least one element.</returns>
    public static IWarning[] EnsureWarnings(params IWarning[]? warnings)
    {
        if (warnings == null || warnings.Length == 0)
        {
            return [UnknownWarning.Instance];
        }
        return warnings;
    }
}
