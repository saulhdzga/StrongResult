namespace StrongResult.Test.Common;

using StrongResult.Common;

public class ErrorValidationTests
{
    [Fact]
    public void Create_WithValidParameters_ReturnsError()
    {
        // Act
        var error = Error.Create("E001", "An error occurred");

        // Assert
        Assert.NotNull(error);
        Assert.Equal("E001", error.Code);
        Assert.Equal("An error occurred", error.Message);
    }

    [Fact]
    public void Create_WithNullCode_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Error.Create(null!, "message"));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void Create_WithEmptyCode_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Error.Create(string.Empty, "message"));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void Create_WithWhitespaceCode_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Error.Create("   ", "message"));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void Create_WithNullMessage_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Error.Create("E001", null!));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Create_WithEmptyMessage_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Error.Create("E001", string.Empty));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Create_WithWhitespaceMessage_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Error.Create("E001", "   "));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void FromException_WithValidException_ReturnsError()
    {
        // Arrange
        var exception = new InvalidOperationException("Something went wrong");

        // Act
        var error = Error.FromException(exception);

        // Assert
        Assert.NotNull(error);
        Assert.Equal("InvalidOperationException", error.Code);
        Assert.Equal("Something went wrong", error.Message);
    }

    [Fact]
    public void FromException_WithNullException_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Error.FromException(null!));
        Assert.Equal("ex", exception.ParamName);
    }
}
