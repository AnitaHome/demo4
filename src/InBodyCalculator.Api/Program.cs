using InBodyCalculator.Api.Models.Requests;
using InBodyCalculator.Api.Models.Responses;
using InBodyCalculator.Api.Services;
using InBodyCalculator.Api.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICalculatorService, CalculatorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

var calculate = app.MapGroup("/api/calculate")
	.WithTags("Calculations");

calculate.MapPost("/bmi", (BmiRequest request, ICalculatorService service) =>
		Results.Ok(service.CalculateBmi(request.Height, request.Weight)))
	.AddEndpointFilter<ValidationFilter<BmiRequest>>()
	.WithName("CalculateBmi")
	.WithSummary("Calculate body mass index and Asian BMI category")
	.Produces<BmiResponse>()
	.ProducesValidationProblem();

calculate.MapPost("/bmr", (BmrRequest request, ICalculatorService service) =>
		Results.Ok(service.CalculateBmr(
			request.Height,
			request.Weight,
			checked((int)request.Age),
			request.Gender!)))
	.AddEndpointFilter<ValidationFilter<BmrRequest>>()
	.WithName("CalculateBmr")
	.WithSummary("Calculate basal metabolic rate using Mifflin-St Jeor")
	.Produces<BmrResponse>()
	.ProducesValidationProblem();

calculate.MapPost("/tdee", (TdeeRequest request, ICalculatorService service) =>
		Results.Ok(service.CalculateTdee(
			request.Height,
			request.Weight,
			checked((int)request.Age),
			request.Gender!,
			request.ActivityLevel!)))
	.AddEndpointFilter<ValidationFilter<TdeeRequest>>()
	.WithName("CalculateTdee")
	.WithSummary("Calculate daily energy expenditure for an activity level")
	.Produces<TdeeResponse>()
	.ProducesValidationProblem();

app.MapFallbackToFile("index.html");

app.Run();

public partial class Program;
