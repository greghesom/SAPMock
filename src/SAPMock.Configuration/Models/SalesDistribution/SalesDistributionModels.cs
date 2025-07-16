using System.ComponentModel.DataAnnotations;

namespace SAPMock.Configuration.Models.SalesDistribution;

#region Customer Models

/// <summary>
/// Request model for creating a new customer.
/// </summary>
public class CreateCustomerRequest
{
    /// <summary>
    /// Customer Number (KUNNR) - If not provided, will be auto-generated.
    /// </summary>
    [StringLength(10)]
    public string? CustomerNumber { get; set; }

    /// <summary>
    /// Customer Name (NAME1) - Name of the customer.
    /// </summary>
    [Required]
    [StringLength(35)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Customer Name 2 (NAME2) - Additional name field.
    /// </summary>
    [StringLength(35)]
    public string Name2 { get; set; } = string.Empty;

    /// <summary>
    /// Search Term (SORT1) - Search term for customer.
    /// </summary>
    [StringLength(20)]
    public string SearchTerm { get; set; } = string.Empty;

    /// <summary>
    /// City (ORT01) - City of the customer.
    /// </summary>
    [StringLength(35)]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Postal Code (PSTLZ) - Postal code of the customer.
    /// </summary>
    [StringLength(10)]
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// Country (LAND1) - Country code of the customer.
    /// </summary>
    [StringLength(3)]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Region (REGIO) - Region of the customer.
    /// </summary>
    [StringLength(3)]
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// Street (STRAS) - Street address of the customer.
    /// </summary>
    [StringLength(35)]
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Customer Group (KTOKD) - Customer account group.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string CustomerGroup { get; set; } = string.Empty;

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
    /// Currency (WAERS) - Currency for the customer.
    /// </summary>
    [StringLength(5)]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Payment Terms (ZTERM) - Payment terms.
    /// </summary>
    [StringLength(4)]
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// Credit Limit (KLIM) - Credit limit for the customer.
    /// </summary>
    public decimal CreditLimit { get; set; }

    /// <summary>
    /// Telephone (TELF1) - Telephone number.
    /// </summary>
    [StringLength(16)]
    public string Telephone { get; set; } = string.Empty;

    /// <summary>
    /// Email (SMTP_ADDR) - Email address.
    /// </summary>
    [StringLength(241)]
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Request model for updating an existing customer.
/// </summary>
public class UpdateCustomerRequest
{
    /// <summary>
    /// Customer Name (NAME1) - Name of the customer.
    /// </summary>
    [StringLength(35)]
    public string? Name { get; set; }

    /// <summary>
    /// Customer Name 2 (NAME2) - Additional name field.
    /// </summary>
    [StringLength(35)]
    public string? Name2 { get; set; }

    /// <summary>
    /// Search Term (SORT1) - Search term for customer.
    /// </summary>
    [StringLength(20)]
    public string? SearchTerm { get; set; }

    /// <summary>
    /// City (ORT01) - City of the customer.
    /// </summary>
    [StringLength(35)]
    public string? City { get; set; }

    /// <summary>
    /// Postal Code (PSTLZ) - Postal code of the customer.
    /// </summary>
    [StringLength(10)]
    public string? PostalCode { get; set; }

    /// <summary>
    /// Country (LAND1) - Country code of the customer.
    /// </summary>
    [StringLength(3)]
    public string? Country { get; set; }

    /// <summary>
    /// Region (REGIO) - Region of the customer.
    /// </summary>
    [StringLength(3)]
    public string? Region { get; set; }

    /// <summary>
    /// Street (STRAS) - Street address of the customer.
    /// </summary>
    [StringLength(35)]
    public string? Street { get; set; }

    /// <summary>
    /// Currency (WAERS) - Currency for the customer.
    /// </summary>
    [StringLength(5)]
    public string? Currency { get; set; }

    /// <summary>
    /// Payment Terms (ZTERM) - Payment terms.
    /// </summary>
    [StringLength(4)]
    public string? PaymentTerms { get; set; }

    /// <summary>
    /// Credit Limit (KLIM) - Credit limit for the customer.
    /// </summary>
    public decimal? CreditLimit { get; set; }

    /// <summary>
    /// Telephone (TELF1) - Telephone number.
    /// </summary>
    [StringLength(16)]
    public string? Telephone { get; set; }

    /// <summary>
    /// Email (SMTP_ADDR) - Email address.
    /// </summary>
    [StringLength(241)]
    public string? Email { get; set; }
}

/// <summary>
/// Response model for customer operations.
/// </summary>
public class CustomerResponse
{
    /// <summary>
    /// Customer Number (KUNNR) - Unique identifier for the customer.
    /// </summary>
    public string CustomerNumber { get; set; } = string.Empty;

    /// <summary>
    /// Customer Name (NAME1) - Name of the customer.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Customer Name 2 (NAME2) - Additional name field.
    /// </summary>
    public string Name2 { get; set; } = string.Empty;

    /// <summary>
    /// Search Term (SORT1) - Search term for customer.
    /// </summary>
    public string SearchTerm { get; set; } = string.Empty;

    /// <summary>
    /// City (ORT01) - City of the customer.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Postal Code (PSTLZ) - Postal code of the customer.
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// Country (LAND1) - Country code of the customer.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Region (REGIO) - Region of the customer.
    /// </summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// Street (STRAS) - Street address of the customer.
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Customer Group (KTOKD) - Customer account group.
    /// </summary>
    public string CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// Sales Organization (VKORG) - Sales organization.
    /// </summary>
    public string SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// Distribution Channel (VTWEG) - Distribution channel.
    /// </summary>
    public string DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// Division (SPART) - Division.
    /// </summary>
    public string Division { get; set; } = string.Empty;

    /// <summary>
    /// Currency (WAERS) - Currency for the customer.
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Payment Terms (ZTERM) - Payment terms.
    /// </summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// Credit Limit (KLIM) - Credit limit for the customer.
    /// </summary>
    public decimal CreditLimit { get; set; }

    /// <summary>
    /// Telephone (TELF1) - Telephone number.
    /// </summary>
    public string Telephone { get; set; } = string.Empty;

    /// <summary>
    /// Email (SMTP_ADDR) - Email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Created On (ERDAT) - Date when the customer was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Created By (ERNAM) - User who created the customer.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last Changed On (LAEDA) - Date when the customer was last changed.
    /// </summary>
    public DateTime LastChangedOn { get; set; }

    /// <summary>
    /// Last Changed By (AENAM) - User who last changed the customer.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LOEVM) - Indicates if the customer is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;

    /// <summary>
    /// Blocked Flag (LIFSP) - Indicates if the customer is blocked.
    /// </summary>
    public bool BlockedFlag { get; set; } = false;
}

/// <summary>
/// Response model for paginated customer lists.
/// </summary>
public class CustomerListResponse
{
    /// <summary>
    /// List of customers.
    /// </summary>
    public List<CustomerResponse> Customers { get; set; } = new();

    /// <summary>
    /// Total number of customers.
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

#endregion

#region Sales Order Models

/// <summary>
/// Request model for creating a new sales order.
/// </summary>
public class CreateSalesOrderRequest
{
    /// <summary>
    /// Sales Order Number (VBELN) - If not provided, will be auto-generated.
    /// </summary>
    [StringLength(10)]
    public string? SalesOrderNumber { get; set; }

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
    /// Requested Delivery Date (VDATU) - Requested delivery date.
    /// </summary>
    public DateTime? RequestedDeliveryDate { get; set; }

    /// <summary>
    /// Currency (WAERK) - Currency for the sales order.
    /// </summary>
    [Required]
    [StringLength(5)]
    public string Currency { get; set; } = string.Empty;

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
    /// Sales Order Items - List of items in the sales order.
    /// </summary>
    public List<CreateSalesOrderItemRequest> Items { get; set; } = new();
}

/// <summary>
/// Request model for creating a sales order item.
/// </summary>
public class CreateSalesOrderItemRequest
{
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
    /// Order Quantity (KWMENG) - Ordered quantity.
    /// </summary>
    [Required]
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// Sales Unit (VRKME) - Sales unit of measure.
    /// </summary>
    [Required]
    [StringLength(3)]
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// Plant (WERKS) - Plant for the item.
    /// </summary>
    [StringLength(4)]
    public string Plant { get; set; } = string.Empty;

    /// <summary>
    /// Requested Delivery Date (VDATU) - Requested delivery date for the item.
    /// </summary>
    public DateTime? RequestedDeliveryDate { get; set; }
}

/// <summary>
/// Response model for sales order operations.
/// </summary>
public class SalesOrderResponse
{
    /// <summary>
    /// Sales Order Number (VBELN) - Unique identifier for the sales order.
    /// </summary>
    public string SalesOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Type (AUART) - Type of sales order.
    /// </summary>
    public string SalesOrderType { get; set; } = string.Empty;

    /// <summary>
    /// Customer Number (KUNNR) - Sold-to party customer number.
    /// </summary>
    public string CustomerNumber { get; set; } = string.Empty;

    /// <summary>
    /// Purchase Order Number (BSTNK) - Customer's purchase order number.
    /// </summary>
    public string PurchaseOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Purchase Order Date (BSTDK) - Customer's purchase order date.
    /// </summary>
    public DateTime? PurchaseOrderDate { get; set; }

    /// <summary>
    /// Sales Organization (VKORG) - Sales organization.
    /// </summary>
    public string SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// Distribution Channel (VTWEG) - Distribution channel.
    /// </summary>
    public string DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// Division (SPART) - Division.
    /// </summary>
    public string Division { get; set; } = string.Empty;

    /// <summary>
    /// Order Date (AUDAT) - Date when the order was created.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Requested Delivery Date (VDATU) - Requested delivery date.
    /// </summary>
    public DateTime? RequestedDeliveryDate { get; set; }

    /// <summary>
    /// Currency (WAERK) - Currency for the sales order.
    /// </summary>
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
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// Incoterms (INCO1) - Incoterms.
    /// </summary>
    public string Incoterms { get; set; } = string.Empty;

    /// <summary>
    /// Incoterms Location (INCO2) - Incoterms location.
    /// </summary>
    public string IncotermsLocation { get; set; } = string.Empty;

    /// <summary>
    /// Order Status (GBSTK) - Overall status of the order.
    /// </summary>
    public string OrderStatus { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Status (LFSTK) - Delivery status.
    /// </summary>
    public string DeliveryStatus { get; set; } = string.Empty;

    /// <summary>
    /// Billing Status (FKSTK) - Billing status.
    /// </summary>
    public string BillingStatus { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Items - List of items in the sales order.
    /// </summary>
    public List<SalesOrderItemResponse> Items { get; set; } = new();

    /// <summary>
    /// Created On (ERDAT) - Date when the sales order was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Created By (ERNAM) - User who created the sales order.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last Changed On (AEDAT) - Date when the sales order was last changed.
    /// </summary>
    public DateTime LastChangedOn { get; set; }

    /// <summary>
    /// Last Changed By (AENAM) - User who last changed the sales order.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the sales order is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}

/// <summary>
/// Response model for sales order item operations.
/// </summary>
public class SalesOrderItemResponse
{
    /// <summary>
    /// Item Number (POSNR) - Item number within the sales order.
    /// </summary>
    public string ItemNumber { get; set; } = string.Empty;

    /// <summary>
    /// Material Number (MATNR) - Material number.
    /// </summary>
    public string MaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Material Description (ARKTX) - Material description.
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// Order Quantity (KWMENG) - Ordered quantity.
    /// </summary>
    public decimal OrderQuantity { get; set; }

    /// <summary>
    /// Sales Unit (VRKME) - Sales unit of measure.
    /// </summary>
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
    public string Plant { get; set; } = string.Empty;

    /// <summary>
    /// Storage Location (LGORT) - Storage location.
    /// </summary>
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
    public string ItemStatus { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Status (LFSTA) - Delivery status of the item.
    /// </summary>
    public string DeliveryStatus { get; set; } = string.Empty;

    /// <summary>
    /// Billing Status (FKSTA) - Billing status of the item.
    /// </summary>
    public string BillingStatus { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the item is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}

/// <summary>
/// Response model for paginated sales order lists.
/// </summary>
public class SalesOrderListResponse
{
    /// <summary>
    /// List of sales orders.
    /// </summary>
    public List<SalesOrderResponse> SalesOrders { get; set; } = new();

    /// <summary>
    /// Total number of sales orders.
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

#endregion

#region Delivery Models

/// <summary>
/// Response model for delivery operations.
/// </summary>
public class DeliveryResponse
{
    /// <summary>
    /// Delivery Number (VBELN) - Unique identifier for the delivery.
    /// </summary>
    public string DeliveryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Type (LFART) - Type of delivery.
    /// </summary>
    public string DeliveryType { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Number (VGBEL) - Reference sales order number.
    /// </summary>
    public string SalesOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Ship-to Party (KUNAG) - Customer number for ship-to party.
    /// </summary>
    public string ShipToParty { get; set; } = string.Empty;

    /// <summary>
    /// Sold-to Party (KUNNR) - Customer number for sold-to party.
    /// </summary>
    public string SoldToParty { get; set; } = string.Empty;

    /// <summary>
    /// Shipping Point (VSTEL) - Shipping point.
    /// </summary>
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
    /// Delivery Status (WBSTK) - Overall delivery status.
    /// </summary>
    public string DeliveryStatus { get; set; } = string.Empty;

    /// <summary>
    /// Picking Status (KOSTA) - Picking status.
    /// </summary>
    public string PickingStatus { get; set; } = string.Empty;

    /// <summary>
    /// Packing Status (PKSTK) - Packing status.
    /// </summary>
    public string PackingStatus { get; set; } = string.Empty;

    /// <summary>
    /// Goods Issue Status (WBSTK) - Goods issue status.
    /// </summary>
    public string GoodsIssueStatus { get; set; } = string.Empty;

    /// <summary>
    /// Billing Status (FKSTK) - Billing status.
    /// </summary>
    public string BillingStatus { get; set; } = string.Empty;

    /// <summary>
    /// Route (ROUTE) - Route for delivery.
    /// </summary>
    public string Route { get; set; } = string.Empty;

    /// <summary>
    /// Shipping Condition (VERSB) - Shipping condition.
    /// </summary>
    public string ShippingCondition { get; set; } = string.Empty;

    /// <summary>
    /// Total Weight (BTGEW) - Total weight of the delivery.
    /// </summary>
    public decimal TotalWeight { get; set; }

    /// <summary>
    /// Weight Unit (GEWEI) - Weight unit.
    /// </summary>
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// Total Volume (VOLUM) - Total volume of the delivery.
    /// </summary>
    public decimal TotalVolume { get; set; }

    /// <summary>
    /// Volume Unit (VOLEH) - Volume unit.
    /// </summary>
    public string VolumeUnit { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Items - List of items in the delivery.
    /// </summary>
    public List<DeliveryItemResponse> Items { get; set; } = new();

    /// <summary>
    /// Created On (ERDAT) - Date when the delivery was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Created By (ERNAM) - User who created the delivery.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last Changed On (AEDAT) - Date when the delivery was last changed.
    /// </summary>
    public DateTime LastChangedOn { get; set; }

    /// <summary>
    /// Last Changed By (AENAM) - User who last changed the delivery.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the delivery is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}

/// <summary>
/// Response model for delivery item operations.
/// </summary>
public class DeliveryItemResponse
{
    /// <summary>
    /// Item Number (POSNR) - Item number within the delivery.
    /// </summary>
    public string ItemNumber { get; set; } = string.Empty;

    /// <summary>
    /// Material Number (MATNR) - Material number.
    /// </summary>
    public string MaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Material Description (ARKTX) - Material description.
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Number (VGBEL) - Reference sales order number.
    /// </summary>
    public string SalesOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Item (VGPOS) - Reference sales order item.
    /// </summary>
    public string SalesOrderItem { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Quantity (LFIMG) - Quantity to be delivered.
    /// </summary>
    public decimal DeliveryQuantity { get; set; }

    /// <summary>
    /// Sales Unit (VRKME) - Sales unit of measure.
    /// </summary>
    public string SalesUnit { get; set; } = string.Empty;

    /// <summary>
    /// Plant (WERKS) - Plant for the item.
    /// </summary>
    public string Plant { get; set; } = string.Empty;

    /// <summary>
    /// Storage Location (LGORT) - Storage location.
    /// </summary>
    public string StorageLocation { get; set; } = string.Empty;

    /// <summary>
    /// Batch (CHARG) - Batch number.
    /// </summary>
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
    public string ItemStatus { get; set; } = string.Empty;

    /// <summary>
    /// Picking Status (KOSTA) - Picking status of the item.
    /// </summary>
    public string PickingStatus { get; set; } = string.Empty;

    /// <summary>
    /// Packing Status (PKSTA) - Packing status of the item.
    /// </summary>
    public string PackingStatus { get; set; } = string.Empty;

    /// <summary>
    /// Goods Issue Status (WBSTA) - Goods issue status of the item.
    /// </summary>
    public string GoodsIssueStatus { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the item is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}

/// <summary>
/// Response model for paginated delivery lists.
/// </summary>
public class DeliveryListResponse
{
    /// <summary>
    /// List of deliveries.
    /// </summary>
    public List<DeliveryResponse> Deliveries { get; set; } = new();

    /// <summary>
    /// Total number of deliveries.
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

#endregion

#region Invoice Models

/// <summary>
/// Response model for invoice operations.
/// </summary>
public class InvoiceResponse
{
    /// <summary>
    /// Invoice Number (VBELN) - Unique identifier for the invoice.
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Invoice Type (FKART) - Type of invoice.
    /// </summary>
    public string InvoiceType { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Number (VGBEL) - Reference sales order number.
    /// </summary>
    public string SalesOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Number (VGBEL) - Reference delivery number.
    /// </summary>
    public string DeliveryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Payer (KUNRG) - Customer number for payer.
    /// </summary>
    public string Payer { get; set; } = string.Empty;

    /// <summary>
    /// Sold-to Party (KUNNR) - Customer number for sold-to party.
    /// </summary>
    public string SoldToParty { get; set; } = string.Empty;

    /// <summary>
    /// Bill-to Party (KUNRE) - Customer number for bill-to party.
    /// </summary>
    public string BillToParty { get; set; } = string.Empty;

    /// <summary>
    /// Sales Organization (VKORG) - Sales organization.
    /// </summary>
    public string SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// Distribution Channel (VTWEG) - Distribution channel.
    /// </summary>
    public string DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// Division (SPART) - Division.
    /// </summary>
    public string Division { get; set; } = string.Empty;

    /// <summary>
    /// Invoice Date (FKDAT) - Date of the invoice.
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// Posting Date (BUDAT) - Posting date.
    /// </summary>
    public DateTime PostingDate { get; set; }

    /// <summary>
    /// Due Date (ZFBDT) - Due date for payment.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Currency (WAERK) - Currency for the invoice.
    /// </summary>
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Net Value (NETWR) - Net value of the invoice.
    /// </summary>
    public decimal NetValue { get; set; }

    /// <summary>
    /// Tax Amount (MWSBP) - Tax amount.
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Total Value (KURRF) - Total value including tax.
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Payment Terms (ZTERM) - Payment terms.
    /// </summary>
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// Payment Method (ZLSCH) - Payment method.
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Accounting Document Number (BELNR) - Accounting document number.
    /// </summary>
    public string AccountingDocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Fiscal Year (GJAHR) - Fiscal year.
    /// </summary>
    public string FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// Company Code (BUKRS) - Company code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Invoice Status (FKSTK) - Status of the invoice.
    /// </summary>
    public string InvoiceStatus { get; set; } = string.Empty;

    /// <summary>
    /// Payment Status (ZAHLK) - Payment status.
    /// </summary>
    public string PaymentStatus { get; set; } = string.Empty;

    /// <summary>
    /// Cancelled Flag (STOKZ) - Indicates if the invoice is cancelled.
    /// </summary>
    public bool CancelledFlag { get; set; } = false;

    /// <summary>
    /// Cancellation Invoice Number (SFAKN) - Cancellation invoice number.
    /// </summary>
    public string CancellationInvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Print Status (DRUKZ) - Print status.
    /// </summary>
    public string PrintStatus { get; set; } = string.Empty;

    /// <summary>
    /// EDI Status (EDISTAT) - EDI status.
    /// </summary>
    public string EDIStatus { get; set; } = string.Empty;

    /// <summary>
    /// Invoice Items - List of items in the invoice.
    /// </summary>
    public List<InvoiceItemResponse> Items { get; set; } = new();

    /// <summary>
    /// Created On (ERDAT) - Date when the invoice was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Created By (ERNAM) - User who created the invoice.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last Changed On (AEDAT) - Date when the invoice was last changed.
    /// </summary>
    public DateTime LastChangedOn { get; set; }

    /// <summary>
    /// Last Changed By (AENAM) - User who last changed the invoice.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the invoice is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}

/// <summary>
/// Response model for invoice item operations.
/// </summary>
public class InvoiceItemResponse
{
    /// <summary>
    /// Item Number (POSNR) - Item number within the invoice.
    /// </summary>
    public string ItemNumber { get; set; } = string.Empty;

    /// <summary>
    /// Material Number (MATNR) - Material number.
    /// </summary>
    public string MaterialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Material Description (ARKTX) - Material description.
    /// </summary>
    public string MaterialDescription { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Number (VGBEL) - Reference sales order number.
    /// </summary>
    public string SalesOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Item (VGPOS) - Reference sales order item.
    /// </summary>
    public string SalesOrderItem { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Number (VGBEL) - Reference delivery number.
    /// </summary>
    public string DeliveryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Item (VGPOS) - Reference delivery item.
    /// </summary>
    public string DeliveryItem { get; set; } = string.Empty;

    /// <summary>
    /// Billing Quantity (FKIMG) - Quantity being billed.
    /// </summary>
    public decimal BillingQuantity { get; set; }

    /// <summary>
    /// Sales Unit (VRKME) - Sales unit of measure.
    /// </summary>
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
    /// Tax Code (MWSKZ) - Tax code.
    /// </summary>
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// Tax Rate (MWSBT) - Tax rate.
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Plant (WERKS) - Plant for the item.
    /// </summary>
    public string Plant { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center (PRCTR) - Profit center.
    /// </summary>
    public string ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// Cost Center (KOSTL) - Cost center.
    /// </summary>
    public string CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// GL Account (SAKNR) - General ledger account.
    /// </summary>
    public string GLAccount { get; set; } = string.Empty;

    /// <summary>
    /// Item Status (GBSTA) - Status of the item.
    /// </summary>
    public string ItemStatus { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the item is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}

/// <summary>
/// Response model for paginated invoice lists.
/// </summary>
public class InvoiceListResponse
{
    /// <summary>
    /// List of invoices.
    /// </summary>
    public List<InvoiceResponse> Invoices { get; set; } = new();

    /// <summary>
    /// Total number of invoices.
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

#endregion

#region Common Models

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
    public string MessageClass { get; set; } = "SD";

    /// <summary>
    /// Message number.
    /// </summary>
    public string MessageNumber { get; set; } = "001";

    /// <summary>
    /// Message variables.
    /// </summary>
    public List<string> MessageVariables { get; set; } = new();
}

#endregion