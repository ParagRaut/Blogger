using Blogger.Infrastructure.Caching;
using Blogger.Infrastructure.Data;
using Blogger.Infrastructure.Repository;
using Blogger.UseCases.AuthorUseCases.Repositories;
using Blogger.UseCases.PostUseCases.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blogger.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddPersistance(configuration)
            .AddDistributedCache(configuration);

        return services;
    }

    private static IServiceCollection AddPersistance(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        string? dbConnectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(
            options => options.UseNpgsql(dbConnectionString));

        services.AddScoped<IPostRepository, PostRepository>();
        services.Decorate<IPostRepository, CachedPostRepository>();

        services.AddScoped<IAuthorRepository, AuthorRepository>();

        return services;
    }

    private static IServiceCollection AddDistributedCache(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        string? cacheConnectionString = configuration.GetConnectionString("Cache");

        services.AddStackExchangeRedisCache(
            options =>
                options.Configuration = cacheConnectionString);

        return services;
    }
}
