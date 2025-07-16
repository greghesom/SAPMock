using SAPMock.ServiceDefaults;
using SAPMock.Configuration;
using SAPMock.Core;
using SAPMock.Api.Models;
using SAPMock.Api.Services;
using SAPMock.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add SAP Mock configuration
builder.Services.Configure<SAPMockConfiguration>(
    builder.Configuration.GetSection("SAPMock"));

// Register SAP Mock services
builder.Services.AddSingleton<IConfigurationService>(provider =>
{
    var config = provider.GetRequiredService<Microsoft.Extensions.Options.IOptions<SAPMockConfiguration>>();
    return new ConfigurationService(config.Value);
});
builder.Services.AddSingleton<ISAPSystemRegistry, SAPSystemRegistry>();

// Register endpoint registration service as hosted service
builder.Services.AddHostedService<EndpointRegistrationService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Register dynamic SAP endpoints
await app.RegisterSAPEndpoints();

// SAP Mock endpoints
app.MapGet("/api/systems", async (ISAPSystemRegistry registry) =>
{
    var systems = await registry.GetAllSystemsAsync();
    var response = systems.Select(s => new SystemResponse
    {
        SystemId = s.SystemId,
        Name = s.Name,
        Type = s.Type,
        ConnectionParameters = s.ConnectionParameters
    });
    return Results.Ok(response);
})
.WithName("GetAllSystems")
.WithOpenApi();

app.MapGet("/api/systems/{systemId}", async (string systemId, ISAPSystemRegistry registry) =>
{
    var system = await registry.GetSystem(systemId);
    if (system == null)
        return Results.NotFound();
    
    var response = new SystemResponse
    {
        SystemId = system.SystemId,
        Name = system.Name,
        Type = system.Type,
        ConnectionParameters = system.ConnectionParameters
    };
    return Results.Ok(response);
})
.WithName("GetSystem")
.WithOpenApi();

app.MapGet("/api/systems/{systemId}/modules", async (string systemId, ISAPSystemRegistry registry) =>
{
    var modules = await registry.GetModulesForSystem(systemId);
    var response = modules.Select(m => new ModuleResponse
    {
        ModuleId = m.ModuleId,
        Name = m.Name,
        SystemId = m.SystemId,
        Endpoints = m.Endpoints.Select(e => new EndpointResponse
        {
            Path = e.Path,
            Method = e.Method,
            RequestType = e.RequestType.Name,
            ResponseType = e.ResponseType.Name
        }).ToList()
    });
    return Results.Ok(response);
})
.WithName("GetSystemModules")
.WithOpenApi();

app.MapGet("/api/health", async (ISAPSystemRegistry registry) =>
{
    var healthStatus = await registry.GetSystemHealthStatus();
    var response = new HealthResponse
    {
        Status = "healthy",
        Timestamp = DateTime.UtcNow,
        Systems = healthStatus
    };
    return Results.Ok(response);
})
.WithName("GetHealthStatus")
.WithOpenApi();

app.MapPost("/api/systems", async (SystemResponse systemRequest, ISAPSystemRegistry registry) =>
{
    var system = new SAPSystem
    {
        SystemId = systemRequest.SystemId,
        Name = systemRequest.Name,
        Type = systemRequest.Type,
        ConnectionParameters = systemRequest.ConnectionParameters
    };
    
    await registry.RegisterSystem(system);
    return Results.Created($"/api/systems/{system.SystemId}", systemRequest);
})
.WithName("RegisterSystem")
.WithOpenApi();

app.Run();
