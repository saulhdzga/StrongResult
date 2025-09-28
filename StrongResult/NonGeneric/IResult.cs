using StrongResult.Common;

namespace StrongResult.NonGeneric;

/// <summary>
/// Common interface for all result types, providing properties and methods for result handling.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets a value indicating whether the result represents a successful operation.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents a failed operation.
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    /// Gets the kind of result, indicating the overall outcome of the operation.
    /// </summary>
    ResultKind Kind { get; }

    /// <summary>
    /// Gets the error associated with the result, if any.
    /// </summary>
    IError? Error { get; }

    /// <summary>
    /// Gets the list of warnings associated with the result.
    /// </summary>
    IReadOnlyList<IWarning> Warnings { get; }
}