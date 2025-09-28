using System.ComponentModel;

namespace StrongResult.Common;

/// <summary>
/// Represents the kind of result for an operation.
/// </summary>
public enum ResultKind
{
    /// <summary>
    /// The operation completed successfully without any issues.
    /// </summary>
    [Description("The operation completed successfully without any issues.")]
    HardSuccess,

    /// <summary>
    /// The operation completed with some issues, but was still partially successful.
    /// </summary>
    [Description("The operation completed with some issues, but was still partially successful.")]
    PartialSuccess,

    /// <summary>
    /// The operation encountered a controlled, expected error.
    /// </summary>
    [Description("The operation encountered a controlled, expected error.")]
    ControlledError,

    /// <summary>
    /// The operation failed completely due to an unrecoverable error.
    /// </summary>
    [Description("The operation failed completely due to an unrecoverable error.")]
    HardFailure
}

/// <summary>
/// Provides extension methods for the <see cref="ResultKind"/> enum.
/// </summary>
public static class ResultKindExtensions
{
    /// <summary>
    /// Gets the description attribute of the enum value, if it exists; otherwise, returns the enum value as a string.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The description of the enum value or its string representation.</returns>
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
        return attribute?.Description ?? value.ToString();
    }

    /// <summary>
    /// Gets the string representation of the ResultKind enum value.
    /// </summary>
    /// <param name="kind">The ResultKind enum value.</param>
    public static string toString(this ResultKind kind) => nameof(kind);
}