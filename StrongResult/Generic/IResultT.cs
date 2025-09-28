using StrongResult.NonGeneric;

namespace StrongResult.Generic;

/// <summary>
/// Interface for result types that carry a value.
/// Extends the <see cref="IResult"/> interface to include a value of type <typeparamref name="T"/>.
/// </summary>
public interface IResult<T> : IResult
{
    T? Value { get; }
}