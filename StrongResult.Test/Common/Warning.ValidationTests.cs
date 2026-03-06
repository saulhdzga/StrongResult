namespace StrongResult.Test.Common;

using StrongResult.Common;

public class WarningValidationTests
{
    [Fact]
    public void Create_WithValidParameters_ReturnsWarning()
    {
        // Act
        var warning = Warning.Create("W001", "A warning occurred");

        // Assert
        Assert.NotNull(warning);
        Assert.Equal("W001", warning.Code);
        Assert.Equal("A warning occurred", warning.Message);
    }

    [Fact]
    public void Create_WithNullCode_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Warning.Create(null!, "message"));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void Create_WithEmptyCode_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Warning.Create(string.Empty, "message"));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void Create_WithWhitespaceCode_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Warning.Create("   ", "message"));
        Assert.Equal("code", exception.ParamName);
    }

    [Fact]
    public void Create_WithNullMessage_ThrowsArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Warning.Create("W001", null!));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Create_WithEmptyMessage_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Warning.Create("W001", string.Empty));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Create_WithWhitespaceMessage_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Warning.Create("W001", "   "));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void UnknownWarning_HasExpectedProperties()
    {
        // Act
        var warning = UnknownWarning.Instance;

        // Assert
        Assert.NotNull(warning);
        Assert.Equal("UnknownWarning", warning.Code);
        Assert.Equal("An unknown warning occurred.", warning.Message);
    }

    [Fact]
    public void UnknownWarning_IsSingleton()
    {
        // Act
        var instance1 = UnknownWarning.Instance;
        var instance2 = UnknownWarning.Instance;

        // Assert
        Assert.Same(instance1, instance2);
    }
}
