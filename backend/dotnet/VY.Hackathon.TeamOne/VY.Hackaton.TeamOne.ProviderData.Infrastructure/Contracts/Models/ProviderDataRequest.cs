namespace VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts.Models;

    public record AirportSchedule(
        string Open,
        string Close
    );

    public record TimeData(
        double Jardineria,
        double Equipaje,
        double Coordination
    );

    public record HandlingFunctionHourPrice(
        TimeData FullTime,
        TimeData PartTime
    );

    public record ProviderDataRequest(
        int PartTimeShiftDuration,
        HandlingFunctionHourPrice HandlingFunctionHourPrice,
        AirportSchedule AirportSchedule
    );