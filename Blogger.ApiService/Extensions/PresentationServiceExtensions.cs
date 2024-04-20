using Blogger.ApiService.Middlewares;

namespace Blogger.ApiService.Extensions;

public static class PresentationServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddProblemDetails();

        services.AddTransient<ExceptionHandlingMiddleware>();

        return services;
    }

    public static IServiceCollection AddHealthChecks(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        string? dbConnectionString = configuration.GetConnectionString("Database");

        string? cacheConnectionString = configuration.GetConnectionString("Cache");

        services.AddHealthChecks()
            .AddNpgSql(dbConnectionString!)
            .AddRedis(cacheConnectionString!);

        return services;
    }
}
