using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace SAPMock.Api.ManualTests;

/// <summary>
/// Manual test console application to verify dynamic endpoint registration functionality.
/// </summary>
public class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task Main(string[] args)
    {
        Console.WriteLine("=== SAP Mock Dynamic Endpoint Registration Test ===");
        
        var baseUrl = "http://localhost:5000";
        
        Console.WriteLine($"Testing endpoints at: {baseUrl}");
        Console.WriteLine();

        // Test 1: Check if systems are loaded
        await TestSystemsEndpoint(baseUrl);

        // Test 2: Check if modules are loaded
        await TestModulesEndpoint(baseUrl);

        // Test 3: Test dynamic endpoints
        await TestDynamicEndpoints(baseUrl);

        // Test 4: Test HTTP methods
        await TestHttpMethods(baseUrl);

        Console.WriteLine();
        Console.WriteLine("=== All tests completed ===");
    }

    private static async Task TestSystemsEndpoint(string baseUrl)
    {
        Console.WriteLine("1. Testing systems endpoint...");
        
        try
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/api/systems");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var systems = JsonSerializer.Deserialize<JsonElement[]>(content);
                Console.WriteLine($"   ✓ Found {systems.Length} systems");
                foreach (var system in systems)
                {
                    Console.WriteLine($"     - {system.GetProperty("systemId").GetString()}: {system.GetProperty("name").GetString()}");
                }
            }
            else
            {
                Console.WriteLine($"   ✗ Failed: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ✗ Error: {ex.Message}");
        }
        
        Console.WriteLine();
    }

    private static async Task TestModulesEndpoint(string baseUrl)
    {
        Console.WriteLine("2. Testing modules endpoint...");
        
        try
        {
            var response = await _httpClient.GetAsync($"{baseUrl}/api/systems/ERP01/modules");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var modules = JsonSerializer.Deserialize<JsonElement[]>(content);
                Console.WriteLine($"   ✓ Found {modules.Length} modules for ERP01");
                foreach (var module in modules)
                {
                    var endpoints = module.GetProperty("endpoints").EnumerateArray();
                    Console.WriteLine($"     - {module.GetProperty("moduleId").GetString()}: {endpoints.Count()} endpoints");
                }
            }
            else
            {
                Console.WriteLine($"   ✗ Failed: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ✗ Error: {ex.Message}");
        }
        
        Console.WriteLine();
    }

    private static async Task TestDynamicEndpoints(string baseUrl)
    {
        Console.WriteLine("3. Testing dynamic endpoints...");
        
        var testEndpoints = new[]
        {
            "/api/ERP01/MM/sap/opu/rest/mm/materials",
            "/api/ERP01/MM/sap/opu/rest/mm/materials/123"
        };

        foreach (var endpoint in testEndpoints)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{baseUrl}{endpoint}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"   ✓ GET {endpoint}");
                }
                else
                {
                    Console.WriteLine($"   ✗ GET {endpoint}: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ✗ GET {endpoint}: {ex.Message}");
            }
        }
        
        Console.WriteLine();
    }

    private static async Task TestHttpMethods(string baseUrl)
    {
        Console.WriteLine("4. Testing HTTP methods...");
        
        var testData = new { name = "Test Material", type = "FERT" };
        var json = JsonSerializer.Serialize(testData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Test POST
        try
        {
            var response = await _httpClient.PostAsync($"{baseUrl}/api/ERP01/MM/sap/opu/rest/mm/materials", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("   ✓ POST materials");
            }
            else
            {
                Console.WriteLine($"   ✗ POST materials: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ✗ POST materials: {ex.Message}");
        }

        // Test PUT
        try
        {
            var putContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{baseUrl}/api/ERP01/MM/sap/opu/rest/mm/materials/123", putContent);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("   ✓ PUT materials/123");
            }
            else
            {
                Console.WriteLine($"   ✗ PUT materials/123: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ✗ PUT materials/123: {ex.Message}");
        }

        // Test DELETE
        try
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/api/ERP01/MM/sap/opu/rest/mm/materials/123");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("   ✓ DELETE materials/123");
            }
            else
            {
                Console.WriteLine($"   ✗ DELETE materials/123: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   ✗ DELETE materials/123: {ex.Message}");
        }

        Console.WriteLine();
    }
}