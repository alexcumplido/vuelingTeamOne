using Serilog;
using VY.Hackathon.TeamOne.WebApi.Auth;
using VY.Hackathon.TeamOne.WebApi.DataProvider;
using VY.Hackathon.TeamOne.WebApi.DataProvider.Repository;
using VY.Hackathon.TeamOne.WebApi.Logging;
using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Contracts;
using VY.Hackaton.TeamOne.ProviderData.Infrastructure.Impl;

try
{
    var builder = WebApplication.CreateBuilder(args);

    

    // Add services to the container.
    builder.Services.AddScoped<ApplicationDbContext>();
    builder.Services.AddScoped<ResultSnapshotRepository>();

    builder.Host.UseLogzIoSerilog();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Configuration.AddJsonFile("VY.Hackathon.TeamOne/appsettings.json", optional: true, reloadOnChange: true);

    builder.Services.ConfigureAuth(builder.Configuration);

    builder.Services.AddHttpClient();

    builder.Services.AddHttpClient<IProviderDataService, ProviderDataService>(client =>
    {
        client.BaseAddress = new Uri("http://localhost:8000/");
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    Environment.ExitCode = 1;
    Log.Fatal(e, "Application Execution Error");
    throw;
}