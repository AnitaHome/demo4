using InBodyCalculator.Api.Models.Responses;

namespace InBodyCalculator.Api.Services;

public interface ICalculatorService
{
    BmiResponse CalculateBmi(double height, double weight);

    BmrResponse CalculateBmr(double height, double weight, int age, string gender);

    TdeeResponse CalculateTdee(double height, double weight, int age, string gender, string activityLevel);
}