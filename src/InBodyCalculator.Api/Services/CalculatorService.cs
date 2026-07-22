using InBodyCalculator.Api.Models.Responses;

namespace InBodyCalculator.Api.Services;

public sealed class CalculatorService : ICalculatorService
{
    public BmiResponse CalculateBmi(double height, double weight)
    {
        ValidatePositive(height, nameof(height));
        ValidatePositive(weight, nameof(weight));

        var heightInMeters = height / 100D;
        var rawBmi = weight / (heightInMeters * heightInMeters);
        var category = rawBmi switch
        {
            < 18.5 => "Underweight",
            < 24 => "Normal",
            < 27 => "Overweight",
            _ => "Obese"
        };

        return new BmiResponse(Round(rawBmi), category);
    }

    public BmrResponse CalculateBmr(double height, double weight, int age, string gender)
    {
        ValidatePositive(height, nameof(height));
        ValidatePositive(weight, nameof(weight));
        ValidatePositive(age, nameof(age));
        ArgumentException.ThrowIfNullOrWhiteSpace(gender);

        var adjustment = gender.ToLowerInvariant() switch
        {
            "male" => 5,
            "female" => -161,
            _ => throw new ArgumentException("Gender must be 'male' or 'female'.", nameof(gender))
        };
        var bmr = (10 * weight) + (6.25 * height) - (5 * age) + adjustment;

        return new BmrResponse(Round(bmr));
    }

    public TdeeResponse CalculateTdee(
        double height,
        double weight,
        int age,
        string gender,
        string activityLevel)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(activityLevel);

        var bmr = CalculateBmr(height, weight, age, gender).Bmr;
        var multiplier = activityLevel.ToLowerInvariant() switch
        {
            "sedentary" => 1.2,
            "light" => 1.375,
            "moderate" => 1.55,
            "heavy" => 1.725,
            "athlete" => 1.9,
            _ => throw new ArgumentException("Invalid activity level.", nameof(activityLevel))
        };

        return new TdeeResponse(bmr, Round(bmr * multiplier));
    }

    private static double Round(double value) =>
        Math.Round(value, 2, MidpointRounding.AwayFromZero);

    private static void ValidatePositive(double value, string parameterName)
    {
        if (!double.IsFinite(value) || value <= 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Value must be greater than zero.");
        }
    }
}