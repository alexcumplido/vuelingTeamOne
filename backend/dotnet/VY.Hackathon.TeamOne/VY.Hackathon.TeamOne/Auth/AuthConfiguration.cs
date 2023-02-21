using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VY.Hackathon.TeamOne.WebApi.DataProvider;

namespace VY.Hackathon.TeamOne.WebApi.Auth;

public static class AuthConfiguration
{
    public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        ConfigureJwt(services, configuration);

        return services;
    }

    private static void ConfigureJwt(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("JwtBearerTokenSettings");

        services.Configure<JwtBearerTokenSettings>(jwtSection);

        var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();

        var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtBearerTokenSettings.Issuer,
                    ValidateAudience = false,
                    ValidAudience = jwtBearerTokenSettings.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
}