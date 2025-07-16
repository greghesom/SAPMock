using System.ComponentModel.DataAnnotations;

namespace SAPMock.Configuration.Models.SalesDistribution;

/// <summary>
/// Represents a customer in the SAP Sales & Distribution module.
/// Contains standard SAP customer fields used across various SD processes.
/// </summary>
public class Customer
{
    /// <summary>
    /// Customer Number (KUNNR) - Unique identifier for the customer.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string CustomerNumber { get; set; } = string.Empty;

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
    [StringLength(4)]
    public string CustomerGroup { get; set; } = string.Empty;

    /// <summary>
    /// Sales Organization (VKORG) - Sales organization.
    /// </summary>
    [StringLength(4)]
    public string SalesOrganization { get; set; } = string.Empty;

    /// <summary>
    /// Distribution Channel (VTWEG) - Distribution channel.
    /// </summary>
    [StringLength(2)]
    public string DistributionChannel { get; set; } = string.Empty;

    /// <summary>
    /// Division (SPART) - Division.
    /// </summary>
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

    /// <summary>
    /// Created On (ERDAT) - Date when the customer was created.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Created By (ERNAM) - User who created the customer.
    /// </summary>
    [StringLength(12)]
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last Changed On (LAEDA) - Date when the customer was last changed.
    /// </summary>
    public DateTime LastChangedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Last Changed By (AENAM) - User who last changed the customer.
    /// </summary>
    [StringLength(12)]
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