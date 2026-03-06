using StrongResult.Common;

namespace StrongResult.NonGeneric;

/// <summary>
/// Provides utility extension methods for the non-generic <see cref="Result"/> type.
/// </summary>
public static partial class ResultExtensions
{
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
