using Blogger.ApiService.Endpoints.V1;
using Blogger.ApiService.Extensions;
using Blogger.ApiService.Middlewares;
using Blogger.Infrastructure;
using Blogger.ServiceDefaults;
using Blogger.UseCases;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services
    .AddPresentation()
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices()
    .AddHealthChecks(builder.Configuration);

builder.Host.UseSerilog(
    (context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    app.SeedData();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.UseSerilogRequestLogging();

// Add a redirect from the root of the app to the swagger endpoint
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapGroup("/api/v1/")
    .WithTags("Posts API")
    .MapPostEndpoints();

app.MapDefaultEndpoints();

app.Run();