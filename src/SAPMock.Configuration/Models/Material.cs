using System.ComponentModel.DataAnnotations;

namespace SAPMock.Configuration.Models;

/// <summary>
/// Represents a material in the SAP Materials Management module.
/// Contains standard SAP material fields used across various MM processes.
/// </summary>
public class Material
{
    /// <summary>
    /// Material Number (MATNR) - Unique identifier for the material.
    /// </summary>
    [Required]
    [StringLength(40)]
    public string MaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Material Description (MAKTX) - Short description of the material.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Material Type (MTART) - Defines the type of material (e.g., ROH, HALB, FERT).
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
    /// Created On (ERSDA) - Date when the material was created.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Created By (ERNAM) - User who created the material.
    /// </summary>
    [StringLength(12)]
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last Changed On (LAEDA) - Date when the material was last changed.
    /// </summary>
    public DateTime LastChangedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last Changed By (AENAM) - User who last changed the material.
    /// </summary>
    [StringLength(12)]
    public string LastChangedBy { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LVORM) - Indicates if the material is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;

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