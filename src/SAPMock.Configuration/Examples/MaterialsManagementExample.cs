using SAPMock.Configuration.Handlers;
using SAPMock.Configuration.Models.MaterialsManagement;
using SAPMock.Core;

namespace SAPMock.Configuration.Examples;

/// <summary>
/// Example demonstrating how to use the MaterialsManagementHandler.
/// </summary>
public static class MaterialsManagementExample
{
    /// <summary>
    /// Demonstrates the Materials Management handler operations.
    /// Note: This example requires a concrete implementation of IMockDataProvider
    /// </summary>
    public static async Task RunExampleAsync(IMockDataProvider dataProvider)
    {
        var systemId = "ERP01";
        
        // Create handler
        var handler = new MaterialsManagementHandler(dataProvider, systemId);
        
        Console.WriteLine("=== Materials Management Handler Example ===");
        
        // Get endpoints
        var endpoints = handler.GetEndpoints(systemId);
        Console.WriteLine($"Available endpoints: {endpoints.Count()}");
        foreach (var endpoint in endpoints)
        {
            Console.WriteLine($"  {endpoint.Method} {endpoint.Path}");
        }
        
        // Test material creation
        Console.WriteLine("\n1. Creating a new material...");
        var createRequest = new CreateMaterialRequest
        {
            Description = "Test Material for Steel Rod",
            MaterialType = "ROH", // Raw material
            MaterialGroup = "STEEL",
            BaseUnitOfMeasure = "PC",
            NetWeight = 1.5m,
            GrossWeight = 1.7m,
            WeightUnit = "KG",
            Plant = "1000",
            StorageLocation = "0001",
            PurchasingGroup = "001",
            StandardPrice = 25.50m,
            Currency = "USD",
            Status = "01"
        };
        
        var createResult = await handler.CreateMaterialAsync(createRequest);
        
        if (createResult is MaterialResponse createdMaterial)
        {
            Console.WriteLine($"Material created successfully:");
            Console.WriteLine($"  Material Number: {createdMaterial.MaterialNumber}");
            Console.WriteLine($"  Description: {createdMaterial.Description}");
            Console.WriteLine($"  Material Type: {createdMaterial.MaterialType}");
            Console.WriteLine($"  Standard Price: {createdMaterial.StandardPrice} {createdMaterial.Currency}");
            
            // Test material retrieval
            Console.WriteLine("\n2. Retrieving the created material...");
            var getResult = await handler.GetMaterialAsync(createdMaterial.MaterialNumber);
            
            if (getResult is MaterialResponse retrievedMaterial)
            {
                Console.WriteLine($"Material retrieved successfully:");
                Console.WriteLine($"  Material Number: {retrievedMaterial.MaterialNumber}");
                Console.WriteLine($"  Description: {retrievedMaterial.Description}");
                Console.WriteLine($"  Created On: {retrievedMaterial.CreatedOn}");
            }
            else if (getResult is SAPErrorResponse getError)
            {
                Console.WriteLine($"Error retrieving material: {getError.Message}");
            }
            
            // Test material update
            Console.WriteLine("\n3. Updating the material...");
            var updateRequest = new UpdateMaterialRequest
            {
                Description = "Updated Test Material for Steel Rod",
                StandardPrice = 27.00m,
                MaterialGroup = "STEEL_UPD"
            };
            
            var updateResult = await handler.UpdateMaterialAsync(createdMaterial.MaterialNumber, updateRequest);
            
            if (updateResult is MaterialResponse updatedMaterial)
            {
                Console.WriteLine($"Material updated successfully:");
                Console.WriteLine($"  Material Number: {updatedMaterial.MaterialNumber}");
                Console.WriteLine($"  Description: {updatedMaterial.Description}");
                Console.WriteLine($"  Material Group: {updatedMaterial.MaterialGroup}");
                Console.WriteLine($"  Standard Price: {updatedMaterial.StandardPrice} {updatedMaterial.Currency}");
                Console.WriteLine($"  Last Changed On: {updatedMaterial.LastChangedOn}");
            }
            else if (updateResult is SAPErrorResponse updateError)
            {
                Console.WriteLine($"Error updating material: {updateError.Message}");
            }
            
            // Test material listing
            Console.WriteLine("\n4. Listing materials...");
            var listResult = await handler.ListMaterialsAsync(1, 10);
            
            if (listResult is MaterialListResponse materialList)
            {
                Console.WriteLine($"Found {materialList.TotalCount} materials (showing page {materialList.Page}):");
                foreach (var material in materialList.Materials)
                {
                    Console.WriteLine($"  - {material.MaterialNumber}: {material.Description}");
                }
            }
            else if (listResult is SAPErrorResponse listError)
            {
                Console.WriteLine($"Error listing materials: {listError.Message}");
            }
            
            // Test material deletion
            Console.WriteLine("\n5. Deleting the material...");
            var deleteResult = await handler.DeleteMaterialAsync(createdMaterial.MaterialNumber);
            
            if (deleteResult is not SAPErrorResponse)
            {
                Console.WriteLine($"Material marked for deletion successfully");
                
                // Verify deletion
                var verifyResult = await handler.GetMaterialAsync(createdMaterial.MaterialNumber);
                if (verifyResult is MaterialResponse deletedMaterial)
                {
                    Console.WriteLine($"Material deletion flag: {deletedMaterial.DeletionFlag}");
                }
            }
            else if (deleteResult is SAPErrorResponse deleteError)
            {
                Console.WriteLine($"Error deleting material: {deleteError.Message}");
            }
        }
        else if (createResult is SAPErrorResponse createError)
        {
            Console.WriteLine($"Error creating material: {createError.Message}");
            Console.WriteLine($"Details: {createError.Details}");
        }
        
        Console.WriteLine("\n=== Example completed ===");
    }
}