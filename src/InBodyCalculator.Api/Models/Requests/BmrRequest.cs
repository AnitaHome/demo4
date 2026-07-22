using System.ComponentModel.DataAnnotations;
using InBodyCalculator.Api.Validation;

namespace InBodyCalculator.Api.Models.Requests;

public record BmrRequest : BmiRequest
{
    [Range(1D, int.MaxValue, ErrorMessage = "Age must be greater than 0.")]
    [WholeNumber(ErrorMessage = "Age must be an integer.")]
    public double Age { get; init; }

    [Required(ErrorMessage = "Gender is required.")]
    [RegularExpression("^(?i:male|female)$", ErrorMessage = "Gender must be 'male' or 'female'.")]
    public string? Gender { get; init; }
}