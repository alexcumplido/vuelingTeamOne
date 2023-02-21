using System.Text.Json.Serialization;

namespace VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts.Models;

public class ProviderDataRequest
{
    [JsonPropertyName("full_time_wage_jardinera")]
    public decimal FullTimeWageJardinera { get; set; }

    [JsonPropertyName("part_time_wage_jardinera")]
    public decimal PartTimeWageJardinera { get; set; }

    [JsonPropertyName("full_time_wage_equipaje")]
    public decimal FullTimeWageEquipaje { get; set; }

    [JsonPropertyName("part_time_wage_equipaje")]
    public decimal PartTimeWageEquipaje { get; set; }

    [JsonPropertyName("full_time_wage_coordinacion")]
    public decimal FullTimeWageCoordinacion { get; set; }

    [JsonPropertyName("part_time_wage_coordinacion")]
    public decimal PartTimeWageCoordinacion { get; set; }

    public DateTime Day { get; set; }
}