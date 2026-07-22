using System.ComponentModel.DataAnnotations;

namespace InBodyCalculator.Api.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class WholeNumberAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value is double number && double.IsInteger(number);
}