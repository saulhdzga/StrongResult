using StrongResult.Common;
using System.Diagnostics.CodeAnalysis;

namespace StrongResult.Generic;

/// <summary>
/// Represents the result of an operation that can succeed or fail, optionally returning a value of type <typeparamref name="T"/>.
/// Inherits from <see cref="ResultBase"/>.
/// Implements the <see cref="IResult{T}"/> interface.
/// </summary>
/// <typeparam name="T">The type of the value returned by the operation.</typeparam>
public sealed record Result<T> : ResultBase, IResult<T>
{
    /// <inheritdoc/>
    [MemberNotNullWhen(true, nameof(Value))]
    public override bool IsSuccess { get; }

    /// <inheritdoc/>
    public override ResultKind Kind => GetKind(IsSuccess, Warnings);

    /// <inheritdoc/>
    public override IError? Error { get; }

    /// <inheritdoc/>
    public override IReadOnlyList<IWarning> Warnings { get; }

    /// <summary>
    /// Gets the value returned by the operation if successful; otherwise, the value is undefined and should not be accessed.
    /// Only valid when <see cref="IsSuccess"/> is true.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> record.
    /// </summary>
    /// <param name="isSuccess">Indicates if the result is a success.</param>
    /// <param name="value">The value returned by the operation, if any.</param>
    /// <param name="error">The error, if any.</param>
    /// <param name="warnings">The warnings, if any.</param>
    private Result(bool isSuccess, T? value, IError? error = null, IEnumerable<IWarning>? warnings = null)
    {
        ResultHelpers.ThrowIfSuccessWithError(isSuccess, error);
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        Warnings = warnings?.ToList() ?? [];
    }

    /// <summary>
    /// Creates a successful result with the specified value and no warnings.
    /// </summary>
    /// <param name="value">The value returned by the operation.</param>
    /// <returns>A <see cref="Result{T}"/> representing a hard success.</returns>
    public static Result<T> Ok(T value) => new(true, value);

    /// <summary>
    /// Creates a partially successful result with the specified value and warnings.
    /// If no warnings are provided, an <see cref="UnknownWarning"/> will be added by default.
    /// </summary>
    /// <param name="value">The value returned by the operation.</param>
    /// <param name="warnings">The warnings to include.</param>
    /// <returns>A <see cref="Result{T}"/> representing a partial success.</returns>
    public static Result<T> PartialSuccess(T value, params IWarning[] warnings)
    {
        warnings = ResultHelpers.EnsureWarnings(warnings);
        return new(true, value, null, warnings);
    }

    /// <summary>
    /// Creates a result representing a controlled error with warnings.
    /// If no warnings are provided, an <see cref="UnknownWarning"/> will be added by default.
    /// </summary>
    /// <param name="error">The error to include.</param>
    /// <param name="warnings">The warnings to include.</param>
    /// <returns>A <see cref="Result{T}"/> representing a controlled error.</returns>
    public static Result<T> ControlledError(IError error, params IWarning[] warnings)
    {
        ResultHelpers.ThrowIfArgumentNull(error, nameof(error));
        warnings = ResultHelpers.EnsureWarnings(warnings);
        return new(false, default, error, warnings);
    }

    /// <summary>
    /// Creates a result representing a hard failure with the specified error.
    /// </summary>
    /// <param name="error">The error to include.</param>
    /// <returns>A <see cref="Result{T}"/> representing a hard failure.</returns>
    public static Result<T> Fail(IError error)
    {
        ResultHelpers.ThrowIfArgumentNull(error, nameof(error));
        return new(false, default, error);
    }

    /// <summary>
    /// Creates a result representing a hard failure from an exception.
    /// </summary>
    /// <param name="ex">The exception to include.</param>
    /// <returns>A <see cref="Result{T}"/> representing a hard failure.</returns>
    public static Result<T> Fail(Exception ex)
    {
        ResultHelpers.ThrowIfArgumentNull(ex, nameof(ex));
        return new(false, default, Common.Error.FromException(ex));
    }

    /// <summary>
    /// Creates a result representing a controlled error from an exception with warnings.
    /// If no warnings are provided, an <see cref="UnknownWarning"/> will be added by default.
    /// </summary>
    /// <param name="ex">The exception to include.</param>
    /// <param name="warnings">The warnings to include.</param>
    /// <returns>A <see cref="Result{T}"/> representing a controlled error.</returns>
    public static Result<T> ControlledError(Exception ex, params IWarning[] warnings)
    {
        ResultHelpers.ThrowIfArgumentNull(ex, nameof(ex));
        warnings = ResultHelpers.EnsureWarnings(warnings);
        return new(false, default, Common.Error.FromException(ex), warnings);
    }

    /// <summary>
    /// Implicitly converts a successful <see cref="Result{T}"/> to its value. Throws if the result is a failure.
    /// </summary>
    /// <param name="result">The result to convert.</param>
    public static implicit operator T(Result<T> result)
    {
        if (result.IsFailure)
            throw new InvalidOperationException($"Cannot convert failed Result to {typeof(T)}. Error: {result.Error}");
        return result.Value!;
    }
}
