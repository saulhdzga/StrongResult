namespace StrongResult.Common;

/// <summary>
/// Represents a warning with a code and message.
/// </summary>
public interface IWarning
{
    /// <summary>
    /// Gets the warning code.
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Gets the warning message.
    /// </summary>
    string Message { get; }
}

/// <summary>
/// A record implementation of <see cref="IWarning"/>.
/// </summary>
public record Warning : IWarning
{
    /// <inheritdoc/>
    public string Code { get; }

    /// <inheritdoc/>
    public string Message { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Warning"/> record.
    /// </summary>
    /// <param name="code">The warning code.</param>
    /// <param name="message">The warning message.</param>
    private Warning(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Creates a new instance of <see cref="Warning"/> with the specified code and message.
    /// </summary>
    /// <param name="code">The warning code.</param>
    /// <param name="message">The warning message.</param>
    /// <returns>A new <see cref="Warning"/> instance.</returns>
    public static Warning Create(string code, string message)
        => new(code, message);
}

/// <summary>
/// Represents an unknown warning for fallback scenarios.
/// </summary>
public sealed record class UnknownWarning : IWarning
{
    public static UnknownWarning Instance { get; } = new();
    public string Code => "UnknownWarning";
    public string Message => "An unknown warning occurred.";
    private UnknownWarning() { }
}
