using System.ComponentModel.DataAnnotations;

namespace InBodyCalculator.Api.Models.Requests;

public record BmiRequest
{
    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Height must be greater than 0.")]
    public double Height { get; init; }

    [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
    public double Weight { get; init; }
}