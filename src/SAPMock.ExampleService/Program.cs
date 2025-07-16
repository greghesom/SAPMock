using SAPMock.ServiceDefaults;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HTTP client for SAP Mock service
builder.Services.AddHttpClient("SAPMock", httpClient =>
{
    // This will be configured by Aspire to point to the sap-mock service
    var sapMockUrl = builder.Configuration["SAPMock:BaseUrl"] ?? "http://localhost:5204";
    httpClient.BaseAddress = new Uri(sapMockUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Example endpoint that uses SAP Mock service
app.MapGet("/materials", async (IHttpClientFactory httpClientFactory) =>
{
    var httpClient = httpClientFactory.CreateClient("SAPMock");
    
    try
    {
        var response = await httpClient.GetAsync("/api/ERP01/MM/sap/opu/rest/mm/materials");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var materials = JsonSerializer.Deserialize<object>(content);
            return Results.Ok(new { 
                source = "SAP Mock", 
                data = materials,
                timestamp = DateTime.UtcNow 
            });
        }
        else
        {
            return Results.Problem($"Failed to retrieve materials: {response.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error calling SAP Mock service: {ex.Message}");
    }
})
.WithName("GetMaterials")
.WithOpenApi();

app.MapGet("/materials/{materialId}", async (string materialId, IHttpClientFactory httpClientFactory) =>
{
    var httpClient = httpClientFactory.CreateClient("SAPMock");
    
    try
    {
        var response = await httpClient.GetAsync($"/api/ERP01/MM/sap/opu/rest/mm/materials/{materialId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var material = JsonSerializer.Deserialize<object>(content);
            return Results.Ok(new { 
                source = "SAP Mock", 
                materialId = materialId,
                data = material,
                timestamp = DateTime.UtcNow 
            });
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return Results.NotFound($"Material {materialId} not found");
        }
        else
        {
            return Results.Problem($"Failed to retrieve material {materialId}: {response.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error calling SAP Mock service: {ex.Message}");
    }
})
.WithName("GetMaterial")
.WithOpenApi();

app.MapGet("/health", () => Results.Ok(new { 
    status = "healthy", 
    service = "SAPMock.ExampleService",
    timestamp = DateTime.UtcNow 
}))
.WithName("GetHealth")
.WithOpenApi();

app.Run();
