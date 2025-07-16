using System.ComponentModel.DataAnnotations;

namespace SAPMock.Configuration.Models.MaterialsManagement;

/// <summary>
/// Request model for creating a new material.
/// </summary>
public class CreateMaterialRequest
{
    /// <summary>
    /// Material Number (MATNR) - If not provided, will be auto-generated.
    /// </summary>
    [StringLength(40)]
    public string? MaterialNumber { get; set; }

    /// <summary>
    /// Material Description (MAKTX) - Short description of the material.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Material Type (MTART) - Defines the type of material.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// Material Group (MATKL) - Groups materials for planning and analysis.
    /// </summary>
    [StringLength(9)]
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// Base Unit of Measure (MEINS) - Basic unit for the material.
    /// </summary>
    [Required]
    [StringLength(3)]
    public string BaseUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// Net Weight (NTGEW) - Net weight of the material.
    /// </summary>
    public decimal NetWeight { get; set; }

    /// <summary>
    /// Gross Weight (BRGEW) - Gross weight of the material.
    /// </summary>
    public decimal GrossWeight { get; set; }

    /// <summary>
    /// Weight Unit (GEWEI) - Unit of weight.
    /// </summary>
    [StringLength(3)]
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// Plant/Storage Location (WERKS) - Default plant for the material.
    /// </summary>
    [StringLength(4)]
    public string Plant { get; set; } = string.Empty;

    /// <summary>
    /// Storage Location (LGORT) - Default storage location.
    /// </summary>
    [StringLength(4)]
    public string StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// Purchasing Group (EKGRP) - Responsible purchasing group.
    /// </summary>
    [StringLength(3)]
    public string PurchasingGroup { get; set; } = string.Empty;

    /// <summary>
    /// Standard Price (STPRS) - Standard price of the material.
    /// </summary>
    public decimal StandardPrice { get; set; }

    /// <summary>
    /// Price Unit (PEINH) - Price unit for the standard price.
    /// </summary>
    public int PriceUnit { get; set; } = 1;

    /// <summary>
    /// Currency (WAERS) - Currency for the standard price.
    /// </summary>
    [StringLength(3)]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Status (MSTAE) - Material status.
    /// </summary>
    [StringLength(2)]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Valid From (DATAB) - Date from which the material is valid.
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// Valid To (DATBI) - Date until which the material is valid.
    /// </summary>
    public DateTime? ValidTo { get; set; }
}

/// <summary>
/// Request model for updating an existing material.
/// </summary>
public class UpdateMaterialRequest
{
    /// <summary>
    /// Material Description (MAKTX) - Short description of the material.
    /// </summary>
    [StringLength(100)]
    public string? Description { get; set; }

    /// <summary>
    /// Material Group (MATKL) - Groups materials for planning and analysis.
    /// </summary>
    [StringLength(9)]
    public string? MaterialGroup { get; set; }

    /// <summary>
    /// Base Unit of Measure (MEINS) - Basic unit for the material.
    /// </summary>
    [StringLength(3)]
    public string? BaseUnitOfMeasure { get; set; }

    /// <summary>
    /// Net Weight (NTGEW) - Net weight of the material.
    /// </summary>
    public decimal? NetWeight { get; set; }

    /// <summary>
    /// Gross Weight (BRGEW) - Gross weight of the material.
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// Weight Unit (GEWEI) - Unit of weight.
    /// </summary>
    [StringLength(3)]
    public string? WeightUnit { get; set; }

    /// <summary>
    /// Plant/Storage Location (WERKS) - Default plant for the material.
    /// </summary>
    [StringLength(4)]
    public string? Plant { get; set; }

    /// <summary>
    /// Storage Location (LGORT) - Default storage location.
    /// </summary>
    [StringLength(4)]
    public string? StorageLocation { get; set; }

    /// <summary>
    /// Purchasing Group (EKGRP) - Responsible purchasing group.
    /// </summary>
    [StringLength(3)]
    public string? PurchasingGroup { get; set; }

    /// <summary>
    /// Standard Price (STPRS) - Standard price of the material.
    /// </summary>
    public decimal? StandardPrice { get; set; }

    /// <summary>
    /// Price Unit (PEINH) - Price unit for the standard price.
    /// </summary>
    public int? PriceUnit { get; set; }

    /// <summary>
    /// Currency (WAERS) - Currency for the standard price.
    /// </summary>
    [StringLength(3)]
    public string? Currency { get; set; }

    /// <summary>
    /// Status (MSTAE) - Material status.
    /// </summary>
    [StringLength(2)]
    public string? Status { get; set; }

    /// <summary>
    /// Valid From (DATAB) - Date from which the material is valid.
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// Valid To (DATBI) - Date until which the material is valid.
    /// </summary>
    public DateTime? ValidTo { get; set; }
}

/// <summary>
/// Response model for material operations.
/// </summary>
public class MaterialResponse
{
    /// <summary>
    /// Material Number (MATNR) - Unique identifier for the material.
    /// </summary>
    public string MaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Material Description (MAKTX) - Short description of the material.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Material Type (MTART) - Defines the type of material.
    /// </summary>
    public string MaterialType { get; set; } = string.Empty;

    /// <summary>
    /// Material Group (MATKL) - Groups materials for planning and analysis.
    /// </summary>
    public string MaterialGroup { get; set; } = string.Empty;

    /// <summary>
    /// Base Unit of Measure (MEINS) - Basic unit for the material.
    /// </summary>
    public string BaseUnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// Net Weight (NTGEW) - Net weight of the material.
    /// </summary>
    public decimal NetWeight { get; set; }

    /// <summary>
    /// Gross Weight (BRGEW) - Gross weight of the material.
    /// </summary>
    public decimal GrossWeight { get; set; }

    /// <summary>
    /// Weight Unit (GEWEI) - Unit of weight.
    /// </summary>
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// Plant/Storage Location (WERKS) - Default plant for the material.
    /// </summary>
    public string Plant { get; set; } = string.Empty;

    /// <summary>
    /// Storage Location (LGORT) - Default storage location.
    /// </summary>
    public string StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// Purchasing Group (EKGRP) - Responsible purchasing group.
    /// </summary>
    public string PurchasingGroup { get; set; } = string.Empty;

    /// <summary>
    /// Standard Price (STPRS) - Standard price of the material.
    /// </summary>
    public decimal StandardPrice { get; set; }

    /// <summary>
    /// Price Unit (PEINH) - Price unit for the standard price.
    /// </summary>
    public int PriceUnit { get; set; } = 1;

    /// <summary>
    /// Currency (WAERS) - Currency for the standard price.
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Created On (ERSDA) - Date when the material was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Created By (ERNAM) - User who created the material.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last Changed On (LAEDA) - Date when the material was last changed.
    /// </summary>
    public DateTime LastChangedOn { get; set; }

    /// <summary>
    /// Last Changed By (AENAM) - User who last changed the material.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LVORM) - Indicates if the material is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;

    /// <summary>
    /// Status (MSTAE) - Material status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Valid From (DATAB) - Date from which the material is valid.
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// Valid To (DATBI) - Date until which the material is valid.
    /// </summary>
    public DateTime? ValidTo { get; set; }
}

/// <summary>
/// Response model for paginated material lists.
/// </summary>
public class MaterialListResponse
{
    /// <summary>
    /// List of materials.
    /// </summary>
    public List<MaterialResponse> Materials { get; set; } = new();

    /// <summary>
    /// Total number of materials.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 50;

    /// <summary>
    /// Total number of pages.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Indicates if there are more pages available.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>
    /// Indicates if there are previous pages available.
    /// </summary>
    public bool HasPreviousPage => Page > 1;
}

/// <summary>
/// SAP error response model matching SAP error patterns.
/// </summary>
public class SAPErrorResponse
{
    /// <summary>
    /// Error code.
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>
    /// Error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Error details.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// Error type (e.g., "E" for Error, "W" for Warning, "I" for Information).
    /// </summary>
    public string Type { get; set; } = "E";

    /// <summary>
    /// Message class.
    /// </summary>
    public string MessageClass { get; set; } = "MM";

    /// <summary>
    /// Message number.
    /// </summary>
    public string MessageNumber { get; set; } = "001";

    /// <summary>
    /// Message variables.
    /// </summary>
    public List<string> MessageVariables { get; set; } = new();
}