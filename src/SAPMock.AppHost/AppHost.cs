var builder = DistributedApplication.CreateBuilder(args);

// Configure SAP Mock Service with configurable settings
var sapMockService = builder.AddProject("sap-mock", "../SAPMock.Api/SAPMock.Api.csproj")
    .WithEnvironment("SAPMock__DataPath", builder.Configuration["SAPMock:DataPath"] ?? "../../data")
    .WithEnvironment("SAPMock__ConfigPath", builder.Configuration["SAPMock:ConfigPath"] ?? "../../config")
    .WithEnvironment("SAPMock__EnableExtensions", builder.Configuration["SAPMock:EnableExtensions"] ?? "true")
    .WithEnvironment("SAPMock__ActiveProfile", builder.Configuration["SAPMock:ActiveProfile"] ?? "default")
    .WithHttpEndpoint(port: 5204, name: "sap-http")
    .WithHttpsEndpoint(port: 7000, name: "sap-https");

// Add example dependent service that references the SAP mock
var exampleService = builder.AddProject("example-service", "../SAPMock.ExampleService/SAPMock.ExampleService.csproj")
    .WithReference(sapMockService)
    .WithEnvironment("SAPMock__BaseUrl", sapMockService.GetEndpoint("sap-http"))
    .WithHttpEndpoint(port: 5205, name: "example-http");

builder.Build().Run();
