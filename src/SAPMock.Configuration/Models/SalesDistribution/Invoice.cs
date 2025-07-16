using System.ComponentModel.DataAnnotations;

namespace SAPMock.Configuration.Models.SalesDistribution;

/// <summary>
/// Represents an invoice in the SAP Sales & Distribution module.
/// Contains standard SAP invoice fields used across various SD processes.
/// </summary>
public class Invoice
{
    /// <summary>
    /// Invoice Number (VBELN) - Unique identifier for the invoice.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Invoice Type (FKART) - Type of invoice.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string InvoiceType { get; set; } = string.Empty;

    /// <summary>
    /// Sales Order Number (VGBEL) - Reference sales order number.
    /// </summary>
    [StringLength(10)]
    public string SalesOrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Number (VGBEL) - Reference delivery number.
    /// </summary>
    [StringLength(10)]
    public string DeliveryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Payer (KUNRG) - Customer number for payer.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string Payer { get; set; } = string.Empty;

    /// <summary>
    /// Sold-to Party (KUNNR) - Customer number for sold-to party.
    /// </summary>
    [StringLength(10)]
    public string SoldToParty { get; set; } = string.Empty;

    /// <summary>
    /// Bill-to Party (KUNRE) - Customer number for bill-to party.
    /// </summary>
    [StringLength(10)]
    public string BillToParty { get; set; } = string.Empty;

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
    /// Invoice Date (FKDAT) - Date of the invoice.
    /// </summary>
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Posting Date (BUDAT) - Posting date.
    /// </summary>
    public DateTime PostingDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Due Date (ZFBDT) - Due date for payment.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Currency (WAERK) - Currency for the invoice.
    /// </summary>
    [Required]
    [StringLength(5)]
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
    [StringLength(4)]
    public string PaymentTerms { get; set; } = string.Empty;

    /// <summary>
    /// Payment Method (ZLSCH) - Payment method.
    /// </summary>
    [StringLength(1)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Accounting Document Number (BELNR) - Accounting document number.
    /// </summary>
    [StringLength(10)]
    public string AccountingDocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Fiscal Year (GJAHR) - Fiscal year.
    /// </summary>
    [StringLength(4)]
    public string FiscalYear { get; set; } = string.Empty;

    /// <summary>
    /// Company Code (BUKRS) - Company code.
    /// </summary>
    [StringLength(4)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Invoice Status (FKSTK) - Status of the invoice.
    /// </summary>
    [StringLength(1)]
    public string InvoiceStatus { get; set; } = "A"; // A = Open, B = Partially paid, C = Completely paid

    /// <summary>
    /// Payment Status (ZAHLK) - Payment status.
    /// </summary>
    [StringLength(1)]
    public string PaymentStatus { get; set; } = "A"; // A = Not paid, B = Partially paid, C = Completely paid

    /// <summary>
    /// Cancelled Flag (STOKZ) - Indicates if the invoice is cancelled.
    /// </summary>
    public bool CancelledFlag { get; set; } = false;

    /// <summary>
    /// Cancellation Invoice Number (SFAKN) - Cancellation invoice number.
    /// </summary>
    [StringLength(10)]
    public string CancellationInvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Print Status (DRUKZ) - Print status.
    /// </summary>
    [StringLength(1)]
    public string PrintStatus { get; set; } = "A"; // A = Not printed, B = Printed

    /// <summary>
    /// EDI Status (EDISTAT) - EDI status.
    /// </summary>
    [StringLength(1)]
    public string EDIStatus { get; set; } = "A"; // A = Not sent, B = Sent

    /// <summary>
    /// Invoice Items - List of items in the invoice.
    /// </summary>
    public List<InvoiceItem> Items { get; set; } = new();

    /// <summary>
    /// Created On (ERDAT) - Date when the invoice was created.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Created By (ERNAM) - User who created the invoice.
    /// </summary>
    [StringLength(12)]
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last Changed On (AEDAT) - Date when the invoice was last changed.
    /// </summary>
    public DateTime LastChangedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last Changed By (AENAM) - User who last changed the invoice.
    /// </summary>
    [StringLength(12)]
    public string LastChangedBy { get; set; } = string.Empty;

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the invoice is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}

/// <summary>
/// Represents an invoice item in the SAP Sales & Distribution module.
/// </summary>
public class InvoiceItem
{
    /// <summary>
    /// Invoice Number (VBELN) - Invoice number.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Item Number (POSNR) - Item number within the invoice.
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
    /// Delivery Number (VGBEL) - Reference delivery number.
    /// </summary>
    [StringLength(10)]
    public string DeliveryNumber { get; set; } = string.Empty;

    /// <summary>
    /// Delivery Item (VGPOS) - Reference delivery item.
    /// </summary>
    [StringLength(6)]
    public string DeliveryItem { get; set; } = string.Empty;

    /// <summary>
    /// Billing Quantity (FKIMG) - Quantity being billed.
    /// </summary>
    public decimal BillingQuantity { get; set; }

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
    /// Tax Code (MWSKZ) - Tax code.
    /// </summary>
    [StringLength(2)]
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// Tax Rate (MWSBT) - Tax rate.
    /// </summary>
    public decimal TaxRate { get; set; }

    /// <summary>
    /// Plant (WERKS) - Plant for the item.
    /// </summary>
    [StringLength(4)]
    public string Plant { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center (PRCTR) - Profit center.
    /// </summary>
    [StringLength(10)]
    public string ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// Cost Center (KOSTL) - Cost center.
    /// </summary>
    [StringLength(10)]
    public string CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// GL Account (SAKNR) - General ledger account.
    /// </summary>
    [StringLength(10)]
    public string GLAccount { get; set; } = string.Empty;

    /// <summary>
    /// Item Status (GBSTA) - Status of the item.
    /// </summary>
    [StringLength(1)]
    public string ItemStatus { get; set; } = "A"; // A = Open, B = Partially processed, C = Completely processed

    /// <summary>
    /// Deletion Flag (LOEKZ) - Indicates if the item is marked for deletion.
    /// </summary>
    public bool DeletionFlag { get; set; } = false;
}