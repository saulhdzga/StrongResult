using StrongResult.Common;
using System.Diagnostics.CodeAnalysis;

namespace StrongResult.Generic;

/// <summary>
/// Provides utility extension methods for <see cref="Result{T}"/>.
/// </summary>
public static partial class ResultTExtensions
{
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
