using SAPMock.Configuration.Examples;
using SAPMock.Configuration.Handlers;
using SAPMock.Configuration.Models.MaterialsManagement;
using SAPMock.Data;

namespace SAPMock.TestConsole;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("SAPMock Materials Management Handler Test");
        Console.WriteLine("==========================================");
        
        try
        {
            // Set up mock data provider
            var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "testdata");
            var dataProvider = new FileBasedMockDataProvider(dataPath, true);
            
            // Run the example
            await MaterialsManagementExample.RunExampleAsync(dataProvider);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}