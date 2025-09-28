using StrongResult.Common;

namespace StrongResult.NonGeneric;

/// <summary>
/// Represents the result of an operation, including success/failure, error, and warnings.
/// Inherits from <see cref="ResultBase"/>.
/// Implements the <see cref="IResult"/> interface.
/// Immutable and thread-safe.
/// </summary>
public sealed record Result : ResultBase, IResult
{
    /// <inheritdoc/>
    public override bool IsSuccess { get; }

    /// <inheritdoc/>
    public override ResultKind Kind => GetKind(IsSuccess, Warnings);

    /// <inheritdoc/>
    public override IError? Error { get; }

    /// <inheritdoc/>
    public override IReadOnlyList<IWarning> Warnings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> record.
    /// </summary>
    /// <param name="isSuccess">Indicates if the result is a success.</param>
    /// <param name="error">The error, if any.</param>
    /// <param name="warnings">The warnings, if any.</param>
    private Result(bool isSuccess, IError? error = null, IEnumerable<IWarning>? warnings = null)
    {
        ResultHelpers.ThrowIfSuccessWithError(isSuccess, error);
        IsSuccess = isSuccess;
        Error = error;
        Warnings = warnings?.ToList() ?? [];
    }

    /// <summary>   
    /// Creates a successful result with no warnings.
    /// </summary>
    /// <returns>A <see cref="Result"/> representing a hard success.</returns>
    public static Result Ok()
    {
        return new(true);
    }

    /// <summary>
    /// Creates a partially successful result with the specified warnings.
    /// If no warnings are provided, an <see cref="UnknownWarning"/> will be added by default.
    /// </summary>
    /// <param name="warnings">The warnings to include.</param>
    /// <returns>A <see cref="Result"/> representing a partial success.</returns>
    public static Result PartialSuccess(params IWarning[] warnings)
    {
        warnings = ResultHelpers.EnsureWarnings(warnings);
        return new(true, null, warnings);
    }

    /// <summary>
    /// Creates a result representing a controlled error with warnings.
    /// If no warnings are provided, an <see cref="UnknownWarning"/> will be added by default.
    /// </summary>
    /// <param name="error">The error to include.</param>
    /// <param name="warnings">The warnings to include.</param>
    /// <returns>A <see cref="Result"/> representing a controlled error.</returns>
    public static Result ControlledError(IError error, params IWarning[] warnings)
    {
        ResultHelpers.ThrowIfArgumentNull(error, nameof(error));
        warnings = ResultHelpers.EnsureWarnings(warnings);
        return new(false, error, warnings);
    }

    /// <summary>
    /// Creates a result representing a hard failure with the specified error.
    /// </summary>
    /// <param name="error">The error to include.</param>
    /// <returns>A <see cref="Result"/> representing a hard failure.</returns>
    public static Result Fail(IError error)
    {
        ResultHelpers.ThrowIfArgumentNull(error, nameof(error));
        return new(false, error);
    }

    /// <summary>
    /// Creates a result representing a hard failure from an exception.
    /// </summary>
    /// <param name="ex">The exception to include.</param>
    /// <returns>A <see cref="Result"/> representing a hard failure.</returns>
    public static Result Fail(Exception ex)
    {
        ResultHelpers.ThrowIfArgumentNull(ex, nameof(ex));
        return new(false, Common.Error.FromException(ex));
    }

    /// <summary>
    /// Creates a result representing a controlled error from an exception with warnings.
    /// If no warnings are provided, an <see cref="UnknownWarning"/> will be added by default.
    /// </summary>
    /// <param name="ex">The exception to include.</param>
    /// <param name="warnings">The warnings to include.</param>
    /// <returns>A <see cref="Result"/> representing a controlled error.</returns>
    public static Result ControlledError(Exception ex, params IWarning[] warnings)
    {
        ResultHelpers.ThrowIfArgumentNull(ex, nameof(ex));
        warnings = ResultHelpers.EnsureWarnings(warnings);
        return new(false, Common.Error.FromException(ex), warnings);
    }
}
