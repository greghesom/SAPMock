using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

Console.WriteLine("Starting SAP Mock Service...");

// First, try to use Aspire if DCP tools are available
bool aspireSucceeded = false;
try
{
    Console.WriteLine("Attempting Aspire orchestration...");
    
    // Create a simple check for DCP availability before attempting full Aspire startup
    var aspireBuilder = Aspire.Hosting.DistributedApplication.CreateBuilder(args);
    var aspireApp = aspireBuilder.Build();
    
    // This will fail if DCP is not available, which is caught below
    await aspireApp.StartAsync();
    Console.WriteLine("Aspire orchestration started successfully!");
    aspireSucceeded = true;
    
    await aspireApp.WaitForShutdownAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Aspire orchestration not available: {ex.Message}");
}

// If Aspire failed, fall back to direct service hosting
if (!aspireSucceeded)
{
    Console.WriteLine("Using direct service hosting approach...");
    
    var builder = Host.CreateApplicationBuilder(args);
    
    // Configure logging
    builder.Logging.AddConsole();
    
    // Add service configuration
    builder.Services.Configure<SAPMockConfig>(options =>
    {
        options.DataPath = builder.Configuration["SAPMock:DataPath"] ?? "./data";
        options.ConfigPath = builder.Configuration["SAPMock:ConfigPath"] ?? "./config";
        options.EnableExtensions = true;
        options.ActiveProfile = builder.Configuration["SAPMock:Profile"] ?? "default";
    });
    
    // Add background service to start the API
    builder.Services.AddHostedService<SAPMockApiService>();
    
    var app = builder.Build();
    
    Console.WriteLine("SAP Mock Service configured for direct hosting!");
    await app.RunAsync();
}

// Configuration class for SAP Mock settings
public class SAPMockConfig
{
    public string DataPath { get; set; } = "./data";
    public string ConfigPath { get; set; } = "./config";
    public bool EnableExtensions { get; set; } = true;
    public string ActiveProfile { get; set; } = "default";
}

// Background service to manage the SAP Mock API process
public class SAPMockApiService : BackgroundService
{
    private readonly ILogger<SAPMockApiService> _logger;
    private Process? _apiProcess;

    public SAPMockApiService(ILogger<SAPMockApiService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Starting SAP Mock API process...");
            
            var apiStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --project src/SAPMock.Api",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = "/home/runner/work/SAPMock/SAPMock"
            };

            _apiProcess = Process.Start(apiStartInfo);
            
            if (_apiProcess != null)
            {
                _logger.LogInformation("SAP Mock API started successfully!");
                _logger.LogInformation("API is running on: http://localhost:5204");
                _logger.LogInformation("Swagger UI: http://localhost:5204/swagger");
                
                // Monitor the process output
                _ = Task.Run(async () =>
                {
                    while (!_apiProcess.HasExited && !stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var output = await _apiProcess.StandardOutput.ReadLineAsync();
                            if (!string.IsNullOrEmpty(output))
                            {
                                _logger.LogInformation("API: {Output}", output);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning("Error reading API output: {Error}", ex.Message);
                            break;
                        }
                    }
                }, stoppingToken);
                
                await _apiProcess.WaitForExitAsync(stoppingToken);
            }
            else
            {
                _logger.LogError("Failed to start the API process.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SAP Mock API service");
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Stopping SAP Mock API service...");
        
        if (_apiProcess != null && !_apiProcess.HasExited)
        {
            try
            {
                _apiProcess.Kill();
                await _apiProcess.WaitForExitAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error stopping API process");
            }
            finally
            {
                _apiProcess?.Dispose();
            }
        }
        
        await base.StopAsync(stoppingToken);
    }
}
