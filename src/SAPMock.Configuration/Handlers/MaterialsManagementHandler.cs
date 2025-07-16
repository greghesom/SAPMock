using System.ComponentModel.DataAnnotations;
using SAPMock.Configuration.Models;
using SAPMock.Configuration.Models.MaterialsManagement;
using SAPMock.Core;

namespace SAPMock.Configuration.Handlers;

/// <summary>
/// Handler for SAP Materials Management (MM) module endpoints.
/// Provides CRUD operations for materials using the mock data provider.
/// </summary>
public class MaterialsManagementHandler : ISAPModuleHandler
{
    private readonly IMockDataProvider _mockDataProvider;
    private readonly string _systemId;
    private readonly List<ISAPEndpoint> _endpoints;

    /// <summary>
    /// Initializes a new instance of the MaterialsManagementHandler.
    /// </summary>
    /// <param name="mockDataProvider">Mock data provider for data storage and retrieval.</param>
    /// <param name="systemId">The SAP system ID this handler is associated with.</param>
    public MaterialsManagementHandler(IMockDataProvider mockDataProvider, string systemId)
    {
        _mockDataProvider = mockDataProvider ?? throw new ArgumentNullException(nameof(mockDataProvider));
        _systemId = systemId ?? throw new ArgumentNullException(nameof(systemId));
        _endpoints = InitializeEndpoints();
    }

    /// <summary>
    /// Gets all endpoints for the Materials Management module.
    /// </summary>
    /// <param name="systemId">The SAP system ID.</param>
    /// <returns>Collection of MM module endpoints.</returns>
    public IEnumerable<ISAPEndpoint> GetEndpoints(string systemId)
    {
        return _endpoints;
    }

    /// <summary>
    /// Initializes the Materials Management module endpoints.
    /// </summary>
    /// <returns>List of configured endpoints.</returns>
    private List<ISAPEndpoint> InitializeEndpoints()
    {
        return new List<ISAPEndpoint>
        {
            // GET /materials/{id}
            new SAPEndpoint
            {
                Path = "/materials/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(MaterialResponse),
                Handler = GetMaterialHandler
            },
            // GET /materials
            new SAPEndpoint
            {
                Path = "/materials",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(MaterialListResponse),
                Handler = ListMaterialsHandler
            },
            // POST /materials
            new SAPEndpoint
            {
                Path = "/materials",
                Method = "POST",
                RequestType = typeof(CreateMaterialRequest),
                ResponseType = typeof(MaterialResponse),
                Handler = CreateMaterialHandler
            },
            // PUT /materials/{id}
            new SAPEndpoint
            {
                Path = "/materials/{id}",
                Method = "PUT",
                RequestType = typeof(UpdateMaterialRequest),
                ResponseType = typeof(MaterialResponse),
                Handler = UpdateMaterialHandler
            },
            // DELETE /materials/{id}
            new SAPEndpoint
            {
                Path = "/materials/{id}",
                Method = "DELETE",
                RequestType = typeof(object),
                ResponseType = typeof(object),
                Handler = DeleteMaterialHandler
            }
        };
    }

    /// <summary>
    /// Retrieves a specific material by its ID.
    /// </summary>
    /// <param name="request">Request object containing material ID.</param>
    /// <returns>Material response or error response.</returns>
    public async Task<object> GetMaterialAsync(string materialId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(materialId))
            {
                return CreateErrorResponse("MM001", "Material ID is required", "Material ID parameter cannot be empty");
            }

            // Get all materials from the collection and find the specific one
            var allMaterials = await GetAllMaterialsAsync();
            var material = allMaterials.FirstOrDefault(m => m.MaterialNumber == materialId);

            if (material == null)
            {
                return CreateErrorResponse("MM002", "Material not found", $"Material with ID {materialId} does not exist");
            }

            return MapToResponse(material);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("MM999", "Internal error", $"An error occurred while retrieving material: {ex.Message}");
        }
    }

    /// <summary>
    /// Lists materials with pagination support.
    /// </summary>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 50).</param>
    /// <param name="materialType">Filter by material type (optional).</param>
    /// <param name="materialGroup">Filter by material group (optional).</param>
    /// <returns>Paginated list of materials.</returns>
    public async Task<object> ListMaterialsAsync(int page = 1, int pageSize = 50, string? materialType = null, string? materialGroup = null)
    {
        try
        {
            // Validate pagination parameters
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 50;

            // For collections, we'll use the GetCollectionAsync method which looks for collection files
            // The FileBasedMockDataProvider will handle the proper key structure internally
            var allMaterials = await GetAllMaterialsAsync();

            // Apply filters
            var filteredMaterials = allMaterials.Where(m => !m.DeletionFlag);

            if (!string.IsNullOrWhiteSpace(materialType))
            {
                filteredMaterials = filteredMaterials.Where(m => m.MaterialType.Equals(materialType, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(materialGroup))
            {
                filteredMaterials = filteredMaterials.Where(m => m.MaterialGroup.Equals(materialGroup, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = filteredMaterials.Count();
            var materials = filteredMaterials
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapToResponse)
                .ToList();

            return new MaterialListResponse
            {
                Materials = materials,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("MM999", "Internal error", $"An error occurred while listing materials: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new material.
    /// </summary>
    /// <param name="request">Create material request.</param>
    /// <returns>Created material response or error response.</returns>
    public async Task<object> CreateMaterialAsync(CreateMaterialRequest request)
    {
        try
        {
            // Validate request
            var validationResult = ValidateCreateRequest(request);
            if (validationResult != null)
                return validationResult;

            // Generate material number if not provided
            var materialNumber = request.MaterialNumber ?? await GenerateMaterialNumberAsync();

            // Get all materials and check if material already exists
            var allMaterials = await GetAllMaterialsAsync();
            var existingMaterial = allMaterials.FirstOrDefault(m => m.MaterialNumber == materialNumber);
            
            if (existingMaterial != null)
            {
                return CreateErrorResponse("MM003", "Material already exists", $"Material with number {materialNumber} already exists");
            }

            var material = new Material
            {
                MaterialNumber = materialNumber,
                Description = request.Description,
                MaterialType = request.MaterialType,
                MaterialGroup = request.MaterialGroup,
                BaseUnitOfMeasure = request.BaseUnitOfMeasure,
                NetWeight = request.NetWeight,
                GrossWeight = request.GrossWeight,
                WeightUnit = request.WeightUnit,
                Plant = request.Plant,
                StorageLocation = request.StorageLocation,
                PurchasingGroup = request.PurchasingGroup,
                StandardPrice = request.StandardPrice,
                PriceUnit = request.PriceUnit,
                Currency = request.Currency,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "SYSTEM", // In a real system, this would be the current user
                LastChangedOn = DateTime.UtcNow,
                LastChangedBy = "SYSTEM",
                Status = request.Status,
                ValidFrom = request.ValidFrom,
                ValidTo = request.ValidTo
            };

            // Add to collection and save
            var materialList = allMaterials.ToList();
            materialList.Add(material);
            await SaveAllMaterialsAsync(materialList);

            return MapToResponse(material);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("MM999", "Internal error", $"An error occurred while creating material: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing material.
    /// </summary>
    /// <param name="materialId">Material ID to update.</param>
    /// <param name="request">Update material request.</param>
    /// <returns>Updated material response or error response.</returns>
    public async Task<object> UpdateMaterialAsync(string materialId, UpdateMaterialRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(materialId))
            {
                return CreateErrorResponse("MM001", "Material ID is required", "Material ID parameter cannot be empty");
            }

            // Validate request
            var validationResult = ValidateUpdateRequest(request);
            if (validationResult != null)
                return validationResult;

            // Get all materials and find the specific one
            var allMaterials = await GetAllMaterialsAsync();
            var material = allMaterials.FirstOrDefault(m => m.MaterialNumber == materialId);

            if (material == null)
            {
                return CreateErrorResponse("MM002", "Material not found", $"Material with ID {materialId} does not exist");
            }

            // Check if material is marked for deletion
            if (material.DeletionFlag)
            {
                return CreateErrorResponse("MM004", "Material is marked for deletion", $"Material {materialId} is marked for deletion and cannot be updated");
            }

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(request.Description))
                material.Description = request.Description;
            if (!string.IsNullOrWhiteSpace(request.MaterialGroup))
                material.MaterialGroup = request.MaterialGroup;
            if (!string.IsNullOrWhiteSpace(request.BaseUnitOfMeasure))
                material.BaseUnitOfMeasure = request.BaseUnitOfMeasure;
            if (request.NetWeight.HasValue)
                material.NetWeight = request.NetWeight.Value;
            if (request.GrossWeight.HasValue)
                material.GrossWeight = request.GrossWeight.Value;
            if (!string.IsNullOrWhiteSpace(request.WeightUnit))
                material.WeightUnit = request.WeightUnit;
            if (!string.IsNullOrWhiteSpace(request.Plant))
                material.Plant = request.Plant;
            if (!string.IsNullOrWhiteSpace(request.StorageLocation))
                material.StorageLocation = request.StorageLocation;
            if (!string.IsNullOrWhiteSpace(request.PurchasingGroup))
                material.PurchasingGroup = request.PurchasingGroup;
            if (request.StandardPrice.HasValue)
                material.StandardPrice = request.StandardPrice.Value;
            if (request.PriceUnit.HasValue)
                material.PriceUnit = request.PriceUnit.Value;
            if (!string.IsNullOrWhiteSpace(request.Currency))
                material.Currency = request.Currency;
            if (!string.IsNullOrWhiteSpace(request.Status))
                material.Status = request.Status;
            if (request.ValidFrom.HasValue)
                material.ValidFrom = request.ValidFrom.Value;
            if (request.ValidTo.HasValue)
                material.ValidTo = request.ValidTo.Value;

            material.LastChangedOn = DateTime.UtcNow;
            material.LastChangedBy = "SYSTEM";

            // Save the updated collection
            await SaveAllMaterialsAsync(allMaterials.ToList());

            return MapToResponse(material);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("MM999", "Internal error", $"An error occurred while updating material: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a material (marks it for deletion).
    /// </summary>
    /// <param name="materialId">Material ID to delete.</param>
    /// <returns>Success response or error response.</returns>
    public async Task<object> DeleteMaterialAsync(string materialId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(materialId))
            {
                return CreateErrorResponse("MM001", "Material ID is required", "Material ID parameter cannot be empty");
            }

            // Get all materials and find the specific one
            var allMaterials = await GetAllMaterialsAsync();
            var material = allMaterials.FirstOrDefault(m => m.MaterialNumber == materialId);

            if (material == null)
            {
                return CreateErrorResponse("MM002", "Material not found", $"Material with ID {materialId} does not exist");
            }

            // Mark for deletion instead of actual deletion (SAP pattern)
            material.DeletionFlag = true;
            material.LastChangedOn = DateTime.UtcNow;
            material.LastChangedBy = "SYSTEM";

            // Save the updated collection
            await SaveAllMaterialsAsync(allMaterials.ToList());

            return new { Success = true, Message = $"Material {materialId} marked for deletion" };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("MM999", "Internal error", $"An error occurred while deleting material: {ex.Message}");
        }
    }

    #region Endpoint Handlers

    private async Task<object> GetMaterialHandler(object request)
    {
        // Extract material ID from request context (this would be handled by the routing framework)
        var materialId = ExtractMaterialIdFromRequest(request);
        return await GetMaterialAsync(materialId);
    }

    private async Task<object> ListMaterialsHandler(object request)
    {
        // Extract query parameters from request context
        var (page, pageSize, materialType, materialGroup) = ExtractListParametersFromRequest(request);
        return await ListMaterialsAsync(page, pageSize, materialType, materialGroup);
    }

    private async Task<object> CreateMaterialHandler(object request)
    {
        if (request is CreateMaterialRequest createRequest)
        {
            return await CreateMaterialAsync(createRequest);
        }
        return CreateErrorResponse("MM005", "Invalid request", "Invalid request format for material creation");
    }

    private async Task<object> UpdateMaterialHandler(object request)
    {
        var materialId = ExtractMaterialIdFromRequest(request);
        if (request is UpdateMaterialRequest updateRequest)
        {
            return await UpdateMaterialAsync(materialId, updateRequest);
        }
        return CreateErrorResponse("MM005", "Invalid request", "Invalid request format for material update");
    }

    private async Task<object> DeleteMaterialHandler(object request)
    {
        var materialId = ExtractMaterialIdFromRequest(request);
        return await DeleteMaterialAsync(materialId);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets all materials from the collection.
    /// </summary>
    private async Task<IEnumerable<Material>> GetAllMaterialsAsync()
    {
        try
        {
            // The FileBasedMockDataProvider GetCollectionAsync uses the pattern default/default/{typeName}
            // We'll use a custom approach to store/retrieve materials as a collection
            var materialsCollection = await _mockDataProvider.GetCollectionAsync<Material>();
            return materialsCollection;
        }
        catch (Exception)
        {
            // If no collection exists, return empty collection
            return new List<Material>();
        }
    }

    /// <summary>
    /// Saves all materials to the collection.
    /// </summary>
    private async Task SaveAllMaterialsAsync(IEnumerable<Material> materials)
    {
        // Store the materials as a collection
        // The FileBasedMockDataProvider will save this as a collection file
        await _mockDataProvider.SaveDataAsync(materials);
    }

    /// <summary>
    /// Maps a Material entity to a MaterialResponse.
    /// </summary>
    private static MaterialResponse MapToResponse(Material material)
    {
        return new MaterialResponse
        {
            MaterialNumber = material.MaterialNumber,
            Description = material.Description,
            MaterialType = material.MaterialType,
            MaterialGroup = material.MaterialGroup,
            BaseUnitOfMeasure = material.BaseUnitOfMeasure,
            NetWeight = material.NetWeight,
            GrossWeight = material.GrossWeight,
            WeightUnit = material.WeightUnit,
            Plant = material.Plant,
            StorageLocation = material.StorageLocation,
            PurchasingGroup = material.PurchasingGroup,
            StandardPrice = material.StandardPrice,
            PriceUnit = material.PriceUnit,
            Currency = material.Currency,
            CreatedOn = material.CreatedOn,
            CreatedBy = material.CreatedBy,
            LastChangedOn = material.LastChangedOn,
            LastChangedBy = material.LastChangedBy,
            DeletionFlag = material.DeletionFlag,
            Status = material.Status,
            ValidFrom = material.ValidFrom,
            ValidTo = material.ValidTo
        };
    }

    /// <summary>
    /// Creates a SAP-formatted error response.
    /// </summary>
    private static SAPErrorResponse CreateErrorResponse(string errorCode, string message, string details)
    {
        return new SAPErrorResponse
        {
            ErrorCode = errorCode,
            Message = message,
            Details = details,
            Type = "E",
            MessageClass = "MM",
            MessageNumber = errorCode,
            MessageVariables = new List<string>()
        };
    }

    /// <summary>
    /// Validates the create material request.
    /// </summary>
    private static SAPErrorResponse? ValidateCreateRequest(CreateMaterialRequest request)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(request, context, results, true))
        {
            var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            return new SAPErrorResponse
            {
                ErrorCode = "MM005",
                Message = "Validation failed",
                Details = errors,
                Type = "E",
                MessageClass = "MM",
                MessageNumber = "005"
            };
        }

        return null;
    }

    /// <summary>
    /// Validates the update material request.
    /// </summary>
    private static SAPErrorResponse? ValidateUpdateRequest(UpdateMaterialRequest request)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(request, context, results, true))
        {
            var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            return new SAPErrorResponse
            {
                ErrorCode = "MM005",
                Message = "Validation failed",
                Details = errors,
                Type = "E",
                MessageClass = "MM",
                MessageNumber = "005"
            };
        }

        return null;
    }

    /// <summary>
    /// Generates a new material number.
    /// </summary>
    private Task<string> GenerateMaterialNumberAsync()
    {
        // Simple incremental generation - in a real system, this would follow SAP numbering rules
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(100, 999);
        return Task.FromResult($"MAT{timestamp}{random}");
    }

    /// <summary>
    /// Extracts material ID from request context.
    /// This is a placeholder - in a real implementation, this would extract from route parameters.
    /// </summary>
    private static string ExtractMaterialIdFromRequest(object request)
    {
        // In a real implementation, this would extract from the HTTP context or route parameters
        // For now, return a placeholder
        return "MAT001";
    }

    /// <summary>
    /// Extracts list parameters from request context.
    /// This is a placeholder - in a real implementation, this would extract from query parameters.
    /// </summary>
    private static (int page, int pageSize, string? materialType, string? materialGroup) ExtractListParametersFromRequest(object request)
    {
        // In a real implementation, this would extract from query parameters
        // For now, return default values
        return (1, 50, null, null);
    }

    #endregion
}