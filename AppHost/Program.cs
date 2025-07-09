using Aspire.Hosting;
using System.Diagnostics;

try
{
    Console.WriteLine("Starting SAP Mock Service with Aspire...");
    
    var builder = DistributedApplication.CreateBuilder(args);

    // Add the SAP Mock API service 
    var sapMockService = builder.AddProject("sap-mock", "src/SAPMock.Api/SAPMock.Api.csproj")
        .WithEnvironment("SAPMock__DataPath", builder.Configuration["SAPMock:DataPath"] ?? "./data")
        .WithEnvironment("SAPMock__ConfigPath", builder.Configuration["SAPMock:ConfigPath"] ?? "./config")
        .WithEnvironment("SAPMock__EnableExtensions", "true")
        .WithEnvironment("SAPMock__ActiveProfile", builder.Configuration["SAPMock:Profile"] ?? "default");

    var app = builder.Build();
    
    Console.WriteLine("Aspire application configured successfully!");
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Aspire orchestration failed: {ex.Message}");
    Console.WriteLine("Falling back to direct API startup...");
    
    // Fallback: Start the API directly
    var apiStartInfo = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = "run --project src/SAPMock.Api",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true
    };

    var apiProcess = Process.Start(apiStartInfo);
    
    if (apiProcess != null)
    {
        Console.WriteLine("SAP Mock API started successfully!");
        Console.WriteLine("API is running on: http://localhost:5204");
        Console.WriteLine("Swagger UI: http://localhost:5204/swagger");
        Console.WriteLine();
        Console.WriteLine("Press Ctrl+C to stop the application...");
        
        await apiProcess.WaitForExitAsync();
    }
    else
    {
        Console.WriteLine("Failed to start the API process.");
    }
}
