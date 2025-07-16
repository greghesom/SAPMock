using System.ComponentModel.DataAnnotations;

namespace SAPMock.Configuration.Models.SalesDistribution;

/// <summary>
/// Represents a sales order in the SAP Sales & Distribution module.
/// Contains standard SAP sales order fields used across various SD processes.
/// </summary>
public class SalesOrder
{
    /// <summary>
    /// Sales Order Number (VBELN) - Unique identifier for the sales order.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string SalesOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Type (AUART) - Type of sales order.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string SalesOrderType { get; set; } = string.Empty;

    /// <summary>
    /// Customer Number (KUNNR) - Sold-to party customer number.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string CustomerNumber { get; set; } = string.Empty;

    /// <summary>
    /// Purchase Order Number (BSTNK) - Customer's purchase order number.
    /// </summary>
    [StringLength(35)]
    public string PurchaseOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Purchase Order Date (BSTDK) - Customer's purchase order date.
    /// </summary>
    public DateTime? PurchaseOrderDate { get; set; }

    /// <summary>
    /// Sales Organization (VKORG) - Sales organization.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// Distribution Channel (VTWEG) - Distribution channel.
    /// </summary>
    [Required]
    [StringLength(2)]
    public string DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// Division (SPART) - Division.
    /// </summary>
    [Required]
    [StringLength(2)]
    public string Division { get; set; } = string.Empty;

    /// <summary>
    /// Sales Group (VKGRP) - Sales group.
    /// </summary>
    [StringLength(3)]
    public string SalesGroup { get; set; } = string.Empty;

    /// <summary>
    /// Sales Office (VKBUR) - Sales office.
    /// </summary>
    [StringLength(4)]
    public string SalesOffice { get; set; } = string.Empty;

    /// <summary>
    /// Order Date (AUDAT) - Date when the order was created.
    /// </summary>
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Requested Delivery Date (VDATU) - Requested delivery date.
    /// </summary>
    public DateTime? RequestedDeliveryDate { get; set; }

    /// <summary>
    /// Price Date (PRSDT) - Date for pricing.
    /// </summary>
    public DateTime? PriceDate { get; set; }

    /// <summary>
    /// Currency (WAERK) - Currency for the sales order.
    /// </summary>
    [Required]
    [StringLength(5)]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Net Value (NETWR) - Net value of the sales order.
    /// </summary>
    public decimal NetValue { get; set; }

    /// <summary>
    /// Tax Amount (MWSBP) - Tax amount.
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Total Value (KWMENG) - Total value including tax.
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Payment Terms (ZTERM) - Payment terms.
    /// </summary>
    [StringLength(4)]
    public string PaymentTerms { get; set; } = string.Empty;

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
    /// Order Status (GBSTK) - Overall status of the order.
    /// </summary>
    [StringLength(1)]
    public string OrderStatus { get; set; } = "A"; // A = Open, B = Partially delivered, C = Completely delivered

    /// <summary>
    /// Delivery Status (LFSTK) - Delivery status.
    /// </summary>
    [StringLength(1)]
    public string DeliveryStatus { get; set; } = "A"; // A = Not delivered, B = Partially delivered, C = Completely delivered

    /// <summary>
    /// Billing Status (FKSTK) - Billing status.
    /// </summary>
    [StringLength(1)]
    public string BillingStatus { get; set; } = "A"; // A = Not billed, B = Partially billed, C = Completely billed

    /// <summary>
    /// Sales Order Items - List of items in the sales order.
    /// </summary>
    public List<SalesOrderItem> Items { get; set; } = new();

    /// <summary>
    /// Created On (ERDAT) - Date when the sales order was created.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Created By (ERNAM) - User who created the sales order.
    /// </summary>
    [StringLength(12)]
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last Changed On (AEDAT) - Date when the sales order was last changed.
    /// </summary>
    public DateTime LastChangedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last Changed By (AENAM) - User who last changed the sales order.
    /// </summary>
    [StringLength(12)]
    public string LastChangedBy { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the sales order is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}

/// <summary>
/// Represents a sales order item in the SAP Sales & Distribution module.
/// </summary>
public class SalesOrderItem
{
    /// <summary>
    /// Sales Order Number (VBELN) - Sales order number.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string SalesOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Item Number (POSNR) - Item number within the sales order.
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
    /// Order Quantity (KWMENG) - Ordered quantity.
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// Sales Unit (VRKME) - Sales unit of measure.
    /// </summary>
    [StringLength(3)]
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// Net Price (NETPR) - Net price per unit.
    /// </summary>
    public decimal NetPrice { get; set; }

    /// <summary>
    /// Price Unit (PEINH) - Price unit.
    /// </summary>
    public int PriceUnit { get; set; } = 1;

    /// <summary>
    /// Net Value (NETWR) - Net value of the item.
    /// </summary>
    public decimal NetValue { get; set; }

    /// <summary>
    /// Tax Amount (MWSBP) - Tax amount for the item.
    /// </summary>
    public decimal TaxAmount { get; set; }

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
    /// Requested Delivery Date (VDATU) - Requested delivery date for the item.
    /// </summary>
    public DateTime? RequestedDeliveryDate { get; set; }

    /// <summary>
    /// Confirmed Delivery Date (EDATU) - Confirmed delivery date.
    /// </summary>
    public DateTime? ConfirmedDeliveryDate { get; set; }

    /// <summary>
    /// Item Status (GBSTA) - Status of the item.
    /// </summary>
    [StringLength(1)]
    public string ItemStatus { get; set; } = "A"; // A = Open, B = Partially delivered, C = Completely delivered

    /// <summary>
    /// Delivery Status (LFSTA) - Delivery status of the item.
    /// </summary>
    [StringLength(1)]
    public string DeliveryStatus { get; set; } = "A"; // A = Not delivered, B = Partially delivered, C = Completely delivered

    /// <summary>
    /// Billing Status (FKSTA) - Billing status of the item.
    /// </summary>
    [StringLength(1)]
    public string BillingStatus { get; set; } = "A"; // A = Not billed, B = Partially billed, C = Completely billed

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the item is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}