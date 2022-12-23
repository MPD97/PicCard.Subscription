using PicCard.Subscription.Api.Services;

namespace PicCard.Subscription.Api;

public static class Extensions
{
    public static void AddLogging(this WebApplicationBuilder builder)
    {
        var applicationName = builder.Configuration["ApplicationName"] ?? "not_set";
        var seqServerUrl = builder.Configuration["SeqServerUrl"] ?? "not_set";
        var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"] ?? "not_set";

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .WriteTo.Seq(seqServerUrl)
            .Enrich.WithProperty("ApplicationName", applicationName)
            .Enrich.WithProperty("Environment", environment)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    public static void AddDapr(this WebApplicationBuilder builder)
    {
        builder.Services.AddDaprClient();
    }

    public static void AddApiAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Bearer");
    }

    public static void AddApiAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "archive");
            });
        });
    }

    public static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }

    public static void UseApiPathBase(this WebApplication application, WebApplicationBuilder builder)
    {
        var applicationName = builder.Configuration["BasePath"];

        if (applicationName is not null)
        {
            application.UsePathBase(applicationName);
        }
    }
}