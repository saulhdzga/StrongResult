using StrongResult.NonGeneric;

namespace StrongResult.Common;

/// <summary>
/// Base record for all result types, providing common properties and methods for result handling.
/// Implements the <see cref="IResult"/> interface.
/// </summary>
public abstract record ResultBase : IResult
{
    /// <inheritdoc/>
    public abstract bool IsSuccess { get; }

    /// <inheritdoc/>
    public bool IsFailure => !IsSuccess;

    /// <inheritdoc/>
    public abstract ResultKind Kind { get; }

    /// <inheritdoc/>
    public abstract IError? Error { get; }

    /// <inheritdoc/>
    public abstract IReadOnlyList<IWarning> Warnings { get; }

    /// <summary>
    /// Determines the kind of result based on success and warnings.
    /// </summary>
    /// <param name="isSuccess">Indicates if the result is a success.</param>
    /// <param name="warnings">The warnings associated with the result.</param>
    /// <returns>The <see cref="ResultKind"/> for the result.</returns>
    protected static ResultKind GetKind(bool isSuccess, IReadOnlyList<IWarning> warnings)
    {
        if (isSuccess)
        {
            return warnings.Count == 0 ? ResultKind.HardSuccess : ResultKind.PartialSuccess;
        }
        else
        {
            return warnings.Count == 0 ? ResultKind.HardFailure : ResultKind.ControlledError;
        }
    }
}