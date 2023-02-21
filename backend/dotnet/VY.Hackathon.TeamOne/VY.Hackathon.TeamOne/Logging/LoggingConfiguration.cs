using Serilog;

namespace VY.Hackathon.TeamOne.WebApi.Logging
{
    public static class LoggingConfiguration
    {
        public static IHostBuilder UseLogzIoSerilog(this IHostBuilder builder)
        {
            return builder
                .UseSerilog((context, configuration) =>
                {
                    configuration.ReadFrom.Configuration(context.Configuration);

                    Log.Information("Application is starting.");
                }, true);
        }
    }
}
