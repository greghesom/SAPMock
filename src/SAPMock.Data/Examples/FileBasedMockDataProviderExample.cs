using System.Text.Json;
using SAPMock.Data;

namespace SAPMock.Data.Examples;

/// <summary>
/// Example program demonstrating the FileBasedMockDataProvider functionality.
/// </summary>
public class FileBasedMockDataProviderExample
{
    /// <summary>
    /// Test data model for demonstration.
    /// </summary>
    public class Customer
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Test data model for collections.
    /// </summary>
    public class Product
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    /// <summary>
    /// Demonstrates the FileBasedMockDataProvider functionality.
    /// </summary>
    public static async Task RunExampleAsync()
    {
        // Create test data directory structure
        var dataPath = Path.Combine(Path.GetTempPath(), "sapmock-test-data");
        
        // Clean up any existing test data
        if (Directory.Exists(dataPath))
        {
            Directory.Delete(dataPath, true);
        }
        
        // Create provider
        var provider = new FileBasedMockDataProvider(dataPath, enableExtensions: true);
        
        Console.WriteLine("=== FileBasedMockDataProvider Example ===");
        Console.WriteLine($"Data Path: {dataPath}");
        Console.WriteLine();
        
        // Create test data
        await CreateTestDataAsync(provider);
        
        // Test single object retrieval
        await TestSingleObjectRetrievalAsync(provider);
        
        // Test collection retrieval
        await TestCollectionRetrievalAsync(provider);
        
        // Test layer fallback
        await TestLayerFallbackAsync(provider);
        
        // Test caching
        await TestCachingAsync(provider);
        
        // Clean up
        if (Directory.Exists(dataPath))
        {
            Directory.Delete(dataPath, true);
        }
        
        Console.WriteLine("\n=== Example completed successfully! ===");
    }

    private static async Task CreateTestDataAsync(FileBasedMockDataProvider provider)
    {
        Console.WriteLine("Creating test data...");
        
        // Create customer data
        var customer = new Customer
        {
            Id = "CUST001",
            Name = "John Doe",
            Email = "john.doe@example.com",
            CreatedAt = DateTime.Now
        };
        
        await provider.SaveDataAsync(customer, "ERP01", "SALES", "customer001");
        
        // Create products collection
        var products = new List<Product>
        {
            new Product { Id = "PROD001", Name = "Widget A", Price = 29.99m, Category = "Electronics" },
            new Product { Id = "PROD002", Name = "Widget B", Price = 39.99m, Category = "Electronics" },
            new Product { Id = "PROD003", Name = "Widget C", Price = 19.99m, Category = "Tools" }
        };
        
        await provider.SaveDataAsync(products, "ERP01", "CATALOG", "products");
        
        Console.WriteLine("✓ Test data created");
    }

    private static async Task TestSingleObjectRetrievalAsync(FileBasedMockDataProvider provider)
    {
        Console.WriteLine("\nTesting single object retrieval...");
        
        try
        {
            var customer = await provider.GetDataAsync<Customer>("ERP01/SALES/customer001");
            Console.WriteLine($"✓ Retrieved customer: {customer.Name} ({customer.Email})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }
    }

    private static async Task TestCollectionRetrievalAsync(FileBasedMockDataProvider provider)
    {
        Console.WriteLine("\nTesting collection retrieval...");
        
        try
        {
            // Test direct collection access
            var products = await provider.GetDataAsync<List<Product>>("ERP01/CATALOG/products");
            Console.WriteLine($"✓ Retrieved {products.Count} products");
            
            foreach (var product in products)
            {
                Console.WriteLine($"  - {product.Name}: ${product.Price}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }
    }

    private static async Task TestLayerFallbackAsync(FileBasedMockDataProvider provider)
    {
        Console.WriteLine("\nTesting layer fallback...");
        
        try
        {
            // Create a provider with extensions disabled to test common layer
            var dataPath = Path.Combine(Path.GetTempPath(), "sapmock-test-data");
            var commonProvider = new FileBasedMockDataProvider(dataPath, enableExtensions: false);
            
            // Create test data in common layer
            var commonCustomer = new Customer
            {
                Id = "CUST002",
                Name = "Jane Doe (Common)",
                Email = "jane.doe@example.com",
                CreatedAt = DateTime.Now
            };
            
            await commonProvider.SaveDataAsync(commonCustomer, "ERP01", "SALES", "customer002");
            
            // Now test with extensions enabled (should fall back to common when extension doesn't exist)
            var customer = await provider.GetDataAsync<Customer>("ERP01/SALES/customer002");
            Console.WriteLine($"✓ Fallback to common layer successful: {customer.Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }
    }

    private static async Task TestCachingAsync(FileBasedMockDataProvider provider)
    {
        Console.WriteLine("\nTesting caching...");
        
        try
        {
            // First retrieval (should load from file)
            var start = DateTime.Now;
            var customer1 = await provider.GetDataAsync<Customer>("ERP01/SALES/customer001");
            var firstLoad = DateTime.Now - start;
            
            // Second retrieval (should load from cache)
            start = DateTime.Now;
            var customer2 = await provider.GetDataAsync<Customer>("ERP01/SALES/customer001");
            var secondLoad = DateTime.Now - start;
            
            Console.WriteLine($"✓ First load: {firstLoad.TotalMilliseconds}ms");
            Console.WriteLine($"✓ Second load: {secondLoad.TotalMilliseconds}ms");
            
            if (secondLoad < firstLoad)
            {
                Console.WriteLine("✓ Caching appears to be working");
            }
            
            // Test cache clearing
            provider.ClearCache();
            Console.WriteLine("✓ Cache cleared");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }
    }
}