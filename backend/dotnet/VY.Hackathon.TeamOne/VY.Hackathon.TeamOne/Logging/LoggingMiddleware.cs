namespace VY.Hackathon.TeamOne.WebApi.Logging;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // it must be a KeyValuePair<string, object> other types does not work
        var stateList = new List<KeyValuePair<string, object>>
        {
            new("user", "mantas@mentalist.dev")
        };

        using var scope = _logger.BeginScope(stateList);
        {
            await _next(context);
        }
    }
}