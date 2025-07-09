using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Add the SAP Mock API service 
var sapMockService = builder.AddProject("sap-mock", "src/SAPMock.Api/SAPMock.Api.csproj")
    .WithEnvironment("SAPMock__DataPath", builder.Configuration["SAPMock:DataPath"] ?? "./data")
    .WithEnvironment("SAPMock__ConfigPath", builder.Configuration["SAPMock:ConfigPath"] ?? "./config")
    .WithEnvironment("SAPMock__EnableExtensions", "true")
    .WithEnvironment("SAPMock__ActiveProfile", builder.Configuration["SAPMock:Profile"] ?? "default");

builder.Build().Run();
