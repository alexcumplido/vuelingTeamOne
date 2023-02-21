namespace VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts.Models;

    public record AirportSchedule(
        string Open,
        string Close
    );

    public record TimeData(
        int Jardineria,
        double Equipaje,
        int Coordination
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