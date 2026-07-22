using InBodyCalculator.Api.Services;

namespace InBodyCalculator.Tests.Services;

public sealed class CalculatorServiceTests
{
    private readonly CalculatorService _service = new();

    [Theory]
    [InlineData(18.49, "Underweight")]
    [InlineData(18.5, "Normal")]
    [InlineData(24, "Overweight")]
    [InlineData(27, "Obese")]
    public void CalculateBmi_CategoryBoundary_ReturnsExpectedCategory(
        double expectedBmi,
        string expectedCategory)
    {
        // Arrange
        const double height = 100;
        var weight = expectedBmi;

        // Act
        var result = _service.CalculateBmi(height, weight);

        // Assert
        Assert.Equal(expectedBmi, result.Bmi);
        Assert.Equal(expectedCategory, result.Category);
    }

    [Theory]
    [InlineData(0, 70)]
    [InlineData(-1, 70)]
    [InlineData(175.5, 0)]
    [InlineData(175.5, -1)]
    public void CalculateBmi_InvalidDimensions_ThrowsArgumentOutOfRangeException(
        double height,
        double weight)
    {
        // Arrange

        // Act
        var action = () => _service.CalculateBmi(height, weight);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Theory]
    [InlineData("male", 1676.88)]
    [InlineData("FEMALE", 1510.88)]
    public void CalculateBmr_ValidGender_ReturnsExpectedValue(string gender, double expected)
    {
        // Arrange

        // Act
        var result = _service.CalculateBmr(175.5, 70, 25, gender);

        // Assert
        Assert.Equal(expected, result.Bmr);
    }

    [Theory]
    [InlineData(0, 70, 25)]
    [InlineData(175.5, 0, 25)]
    [InlineData(175.5, 70, 0)]
    public void CalculateBmr_InvalidMetric_ThrowsArgumentOutOfRangeException(
        double height,
        double weight,
        int age)
    {
        // Arrange

        // Act
        var action = () => _service.CalculateBmr(height, weight, age, "male");

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void CalculateBmr_InvalidGender_ThrowsArgumentException()
    {
        // Arrange

        // Act
        var action = () => _service.CalculateBmr(175.5, 70, 25, "unknown");

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    [Theory]
    [InlineData("sedentary", 2012.26)]
    [InlineData("light", 2305.71)]
    [InlineData("moderate", 2599.16)]
    [InlineData("heavy", 2892.62)]
    [InlineData("ATHLETE", 3186.07)]
    public void CalculateTdee_ActivityLevel_ReturnsExpectedValue(
        string activityLevel,
        double expected)
    {
        // Arrange

        // Act
        var result = _service.CalculateTdee(175.5, 70, 25, "male", activityLevel);

        // Assert
        Assert.Equal(1676.88, result.Bmr);
        Assert.Equal(expected, result.Tdee);
    }

    [Fact]
    public void CalculateTdee_InvalidActivityLevel_ThrowsArgumentException()
    {
        // Arrange

        // Act
        var action = () => _service.CalculateTdee(175.5, 70, 25, "male", "unknown");

        // Assert
        Assert.Throws<ArgumentException>(action);
    }
}