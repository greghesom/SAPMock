using System.ComponentModel.DataAnnotations;

namespace SAPMock.Configuration.Models.SalesDistribution;

/// <summary>
/// Represents a delivery in the SAP Sales & Distribution module.
/// Contains standard SAP delivery fields used across various SD processes.
/// </summary>
public class Delivery
{
    /// <summary>
    /// Delivery Number (VBELN) - Unique identifier for the delivery.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string DeliveryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Type (LFART) - Type of delivery.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string DeliveryType { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Number (VGBEL) - Reference sales order number.
    /// </summary>
    [StringLength(10)]
    public string SalesOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Ship-to Party (KUNAG) - Customer number for ship-to party.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string ShipToParty { get; set; } = string.Empty;

    /// <summary>
    /// Sold-to Party (KUNNR) - Customer number for sold-to party.
    /// </summary>
    [StringLength(10)]
    public string SoldToParty { get; set; } = string.Empty;

    /// <summary>
    /// Shipping Point (VSTEL) - Shipping point.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string ShippingPoint { get; set; } = string.Empty;

    /// <summary>
    /// Planned Goods Movement Date (WADAT_IST) - Planned goods movement date.
    /// </summary>
    public DateTime? PlannedGoodsMovementDate { get; set; }

    /// <summary>
    /// Actual Goods Movement Date (WADAT) - Actual goods movement date.
    /// </summary>
    public DateTime? ActualGoodsMovementDate { get; set; }

    /// <summary>
    /// Delivery Date (LFDAT) - Delivery date.
    /// </summary>
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// Picking Date (KODAT) - Picking date.
    /// </summary>
    public DateTime? PickingDate { get; set; }

    /// <summary>
    /// Loading Date (LDDAT) - Loading date.
    /// </summary>
    public DateTime? LoadingDate { get; set; }

    /// <summary>
    /// Transportation Planning Date (TDDAT) - Transportation planning date.
    /// </summary>
    public DateTime? TransportationPlanningDate { get; set; }

    /// <summary>
    /// Delivery Status (WBSTK) - Overall delivery status.
    /// </summary>
    [StringLength(1)]
    public string DeliveryStatus { get; set; } = "A"; // A = Not yet processed, B = Partially processed, C = Completely processed

    /// <summary>
    /// Picking Status (KOSTA) - Picking status.
    /// </summary>
    [StringLength(1)]
    public string PickingStatus { get; set; } = "A"; // A = Not yet picked, B = Partially picked, C = Completely picked

    /// <summary>
    /// Packing Status (PKSTK) - Packing status.
    /// </summary>
    [StringLength(1)]
    public string PackingStatus { get; set; } = "A"; // A = Not yet packed, B = Partially packed, C = Completely packed

    /// <summary>
    /// Goods Issue Status (WBSTK) - Goods issue status.
    /// </summary>
    [StringLength(1)]
    public string GoodsIssueStatus { get; set; } = "A"; // A = Not yet issued, B = Partially issued, C = Completely issued

    /// <summary>
    /// Billing Status (FKSTK) - Billing status.
    /// </summary>
    [StringLength(1)]
    public string BillingStatus { get; set; } = "A"; // A = Not billed, B = Partially billed, C = Completely billed

    /// <summary>
    /// Route (ROUTE) - Route for delivery.
    /// </summary>
    [StringLength(6)]
    public string Route { get; set; } = string.Empty;

    /// <summary>
    /// Shipping Condition (VERSB) - Shipping condition.
    /// </summary>
    [StringLength(2)]
    public string ShippingCondition { get; set; } = string.Empty;

    /// <summary>
    /// Forwarding Agent (SPEDT) - Forwarding agent.
    /// </summary>
    [StringLength(10)]
    public string ForwardingAgent { get; set; } = string.Empty;

    /// <summary>
    /// Incoterms (INCO1) - Incoterms.
    /// </summary>
    [StringLength(3)]
    public string Incoterms { get; set; } = string.Empty;

    /// <summary>
    /// Incoterms Location (INCO2) - Incoterms location.
    /// </summary>
    [StringLength(28)]
    public string IncotermsLocation { get; set; } = string.Empty;

    /// <summary>
    /// Total Weight (BTGEW) - Total weight of the delivery.
    /// </summary>
    public decimal TotalWeight { get; set; }

    /// <summary>
    /// Weight Unit (GEWEI) - Weight unit.
    /// </summary>
    [StringLength(3)]
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// Total Volume (VOLUM) - Total volume of the delivery.
    /// </summary>
    public decimal TotalVolume { get; set; }

    /// <summary>
    /// Volume Unit (VOLEH) - Volume unit.
    /// </summary>
    [StringLength(3)]
    public string VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Items - List of items in the delivery.
    /// </summary>
    public List<DeliveryItem> Items { get; set; } = new();

    /// <summary>
    /// Created On (ERDAT) - Date when the delivery was created.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Created By (ERNAM) - User who created the delivery.
    /// </summary>
    [StringLength(12)]
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last Changed On (AEDAT) - Date when the delivery was last changed.
    /// </summary>
    public DateTime LastChangedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last Changed By (AENAM) - User who last changed the delivery.
    /// </summary>
    [StringLength(12)]
    public string LastChangedBy { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the delivery is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}

/// <summary>
/// Represents a delivery item in the SAP Sales & Distribution module.
/// </summary>
public class DeliveryItem
{
    /// <summary>
    /// Delivery Number (VBELN) - Delivery number.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string DeliveryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Item Number (POSNR) - Item number within the delivery.
    /// </summary>
    [Required]
    [StringLength(6)]
    public string ItemNumber { get; set; } = string.Empty;

    /// <summary>
    /// Material Number (MATNR) - Material number.
    /// </summary>
    [Required]
    [StringLength(40)]
    public string MaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Material Description (ARKTX) - Material description.
    /// </summary>
    [StringLength(40)]
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Number (VGBEL) - Reference sales order number.
    /// </summary>
    [StringLength(10)]
    public string SalesOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Item (VGPOS) - Reference sales order item.
    /// </summary>
    [StringLength(6)]
    public string SalesOrderItem { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Quantity (LFIMG) - Quantity to be delivered.
    /// </summary>
    public decimal DeliveryQuantity { get; set; }

    /// <summary>
    /// Sales Unit (VRKME) - Sales unit of measure.
    /// </summary>
    [StringLength(3)]
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// Plant (WERKS) - Plant for the item.
    /// </summary>
    [StringLength(4)]
    public string Plant { get; set; } = string.Empty;

    /// <summary>
    /// Storage Location (LGORT) - Storage location.
    /// </summary>
    [StringLength(4)]
    public string StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// Batch (CHARG) - Batch number.
    /// </summary>
    [StringLength(10)]
    public string Batch { get; set; } = string.Empty;

    /// <summary>
    /// Picking Quantity (PIKMG) - Picked quantity.
    /// </summary>
    public decimal PickingQuantity { get; set; }

    /// <summary>
    /// Packed Quantity (VPIMG) - Packed quantity.
    /// </summary>
    public decimal PackedQuantity { get; set; }

    /// <summary>
    /// Issued Quantity (VSIMG) - Goods issued quantity.
    /// </summary>
    public decimal IssuedQuantity { get; set; }

    /// <summary>
    /// Item Status (GBSTA) - Status of the item.
    /// </summary>
    [StringLength(1)]
    public string ItemStatus { get; set; } = "A"; // A = Open, B = Partially processed, C = Completely processed

    /// <summary>
    /// Picking Status (KOSTA) - Picking status of the item.
    /// </summary>
    [StringLength(1)]
    public string PickingStatus { get; set; } = "A"; // A = Not picked, B = Partially picked, C = Completely picked

    /// <summary>
    /// Packing Status (PKSTA) - Packing status of the item.
    /// </summary>
    [StringLength(1)]
    public string PackingStatus { get; set; } = "A"; // A = Not packed, B = Partially packed, C = Completely packed

    /// <summary>
    /// Goods Issue Status (WBSTA) - Goods issue status of the item.
    /// </summary>
    [StringLength(1)]
    public string GoodsIssueStatus { get; set; } = "A"; // A = Not issued, B = Partially issued, C = Completely issued

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the item is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}