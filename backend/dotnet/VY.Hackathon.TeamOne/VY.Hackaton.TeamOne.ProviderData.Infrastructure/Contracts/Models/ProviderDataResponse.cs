namespace VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts.Models;

public record ProviderDataResponse(DateOnly Date, TimeOnly Time, string HandlingArea, int RequiredEmployees, int FullTimeEmployeesCount, int PartTimeEmployeesCount, decimal FullTimeEmployeesCost, decimal PartTimeEmployeesCost, decimal TotalCost);