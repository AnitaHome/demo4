using System.ComponentModel.DataAnnotations;

namespace InBodyCalculator.Api.Models.Requests;

public sealed record TdeeRequest : BmrRequest
{
    [Required(ErrorMessage = "Activity level is required.")]
    [RegularExpression(
        "^(?i:sedentary|light|moderate|heavy|athlete)$",
        ErrorMessage = "Activity level must be sedentary, light, moderate, heavy, or athlete.")]
    public string? ActivityLevel { get; init; }
}