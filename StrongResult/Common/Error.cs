namespace StrongResult.Common;

/// <summary>
/// Represents an error with a code and message.
/// </summary>
public interface IError
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    string Message { get; }
}

/// <summary>
/// Implementation of <see cref="IError"/> as a record.
/// </summary>
public record Error : IError
{
    /// <inheritdoc/>
    public string Code { get; }

    /// <inheritdoc/>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> record.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    private Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified code and message.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    public static Error Create(string code, string message)
        => new(code, message);

    /// <summary>
    /// Creates a new <see cref="Error"/> instance from an <see cref="Exception"/>.
    /// </summary>
    /// <param name="ex">The exception to convert.</param>
    /// <returns>A new <see cref="Error"/> instance representing the exception.</returns>
    public static Error FromException(Exception ex)
        => new(ex.GetType().Name, ex.Message);
}
