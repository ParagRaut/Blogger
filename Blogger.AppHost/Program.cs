using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<RedisResource> cache = builder.AddRedis("cache");

IResourceBuilder<PostgresServerResource> database = builder.AddPostgres("database");

IResourceBuilder<ProjectResource> apiService = builder.AddProject<Blogger_ApiService>("apiservice")
    .WithReference(cache)
    .WithReference(database);

builder.AddProject<Blogger_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiService);

builder.Build().Run();
