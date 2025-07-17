using System.ComponentModel.DataAnnotations;

namespace SAPMock.Configuration.Models.Finance;

#region GL Account Models

/// <summary>
/// Request model for creating or updating a GL account.
/// </summary>
public class CreateGLAccountRequest
{
    /// <summary>
    /// GL Account Number (SAKNR).
    /// </summary>
    [Required]
    [StringLength(10)]
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Account Name - Short description of the account.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Account Type (Assets, Liabilities, Equity, Revenue, Expense).
    /// </summary>
    [Required]
    [StringLength(20)]
    public string AccountType { get; set; } = string.Empty;

    /// <summary>
    /// Account Group - Classification of the account.
    /// </summary>
    [StringLength(4)]
    public string AccountGroup { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Currency for the account.
    /// </summary>
    [Required]
    [StringLength(3)]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Whether the account is blocked for postings.
    /// </summary>
    public bool IsBlocked { get; set; } = false;

    /// <summary>
    /// Whether the account is marked for deletion.
    /// </summary>
    public bool IsMarkedForDeletion { get; set; } = false;
}

/// <summary>
/// GL Account entity representing a General Ledger account.
/// </summary>
public class GLAccount
{
    /// <summary>
    /// GL Account Number (SAKNR).
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Account Name - Short description of the account.
    /// </summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Account Type (Assets, Liabilities, Equity, Revenue, Expense).
    /// </summary>
    public string AccountType { get; set; } = string.Empty;

    /// <summary>
    /// Account Group - Classification of the account.
    /// </summary>
    public string AccountGroup { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Currency for the account.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Current balance of the account.
    /// </summary>
    public decimal Balance { get; set; } = 0;

    /// <summary>
    /// Whether the account is blocked for postings.
    /// </summary>
    public bool IsBlocked { get; set; } = false;

    /// <summary>
    /// Whether the account is marked for deletion.
    /// </summary>
    public bool IsMarkedForDeletion { get; set; } = false;

    /// <summary>
    /// Creation date and time.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who created the account.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last change date and time.
    /// </summary>
    public DateTime LastChangedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who last changed the account.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;
}

/// <summary>
/// Response model for GL Account operations.
/// </summary>
public class GLAccountResponse
{
    /// <summary>
    /// GL Account Number (SAKNR).
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Account Name - Short description of the account.
    /// </summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Account Type (Assets, Liabilities, Equity, Revenue, Expense).
    /// </summary>
    public string AccountType { get; set; } = string.Empty;

    /// <summary>
    /// Account Group - Classification of the account.
    /// </summary>
    public string AccountGroup { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Currency for the account.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Current balance of the account.
    /// </summary>
    public decimal Balance { get; set; } = 0;

    /// <summary>
    /// Whether the account is blocked for postings.
    /// </summary>
    public bool IsBlocked { get; set; } = false;

    /// <summary>
    /// Whether the account is marked for deletion.
    /// </summary>
    public bool IsMarkedForDeletion { get; set; } = false;

    /// <summary>
    /// Creation date and time.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// User who created the account.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last change date and time.
    /// </summary>
    public DateTime LastChangedOn { get; set; }

    /// <summary>
    /// User who last changed the account.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;
}

/// <summary>
/// Response model for GL Account list operations.
/// </summary>
public class GLAccountListResponse
{
    /// <summary>
    /// List of GL accounts.
    /// </summary>
    public List<GLAccountResponse> Accounts { get; set; } = new();

    /// <summary>
    /// Total count of accounts.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; }
}

#endregion

#region Cost Center Models

/// <summary>
/// Request model for creating or updating a cost center.
/// </summary>
public class CreateCostCenterRequest
{
    /// <summary>
    /// Cost Center ID (KOSTL).
    /// </summary>
    [Required]
    [StringLength(10)]
    public string CostCenterNumber { get; set; } = string.Empty;

    /// <summary>
    /// Cost Center Name.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Controlling Area.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string ControllingArea { get; set; } = string.Empty;

    /// <summary>
    /// Responsible Person.
    /// </summary>
    [StringLength(12)]
    public string ResponsiblePerson { get; set; } = string.Empty;

    /// <summary>
    /// Cost Center Hierarchy.
    /// </summary>
    [StringLength(10)]
    public string Hierarchy { get; set; } = string.Empty;

    /// <summary>
    /// Currency for cost center.
    /// </summary>
    [Required]
    [StringLength(3)]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Valid from date.
    /// </summary>
    public DateTime ValidFrom { get; set; } = DateTime.Today;

    /// <summary>
    /// Valid to date.
    /// </summary>
    public DateTime ValidTo { get; set; } = DateTime.MaxValue;
}

/// <summary>
/// Cost Center entity.
/// </summary>
public class CostCenter
{
    /// <summary>
    /// Cost Center ID (KOSTL).
    /// </summary>
    public string CostCenterNumber { get; set; } = string.Empty;

    /// <summary>
    /// Cost Center Name.
    /// </summary>
    public string CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Controlling Area.
    /// </summary>
    public string ControllingArea { get; set; } = string.Empty;

    /// <summary>
    /// Responsible Person.
    /// </summary>
    public string ResponsiblePerson { get; set; } = string.Empty;

    /// <summary>
    /// Cost Center Hierarchy.
    /// </summary>
    public string Hierarchy { get; set; } = string.Empty;

    /// <summary>
    /// Currency for cost center.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Current actual costs.
    /// </summary>
    public decimal ActualCosts { get; set; } = 0;

    /// <summary>
    /// Planned costs.
    /// </summary>
    public decimal PlannedCosts { get; set; } = 0;

    /// <summary>
    /// Valid from date.
    /// </summary>
    public DateTime ValidFrom { get; set; } = DateTime.Today;

    /// <summary>
    /// Valid to date.
    /// </summary>
    public DateTime ValidTo { get; set; } = DateTime.MaxValue;

    /// <summary>
    /// Whether the cost center is blocked.
    /// </summary>
    public bool IsBlocked { get; set; } = false;

    /// <summary>
    /// Creation date and time.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who created the cost center.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last change date and time.
    /// </summary>
    public DateTime LastChangedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who last changed the cost center.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;
}

/// <summary>
/// Response model for Cost Center operations.
/// </summary>
public class CostCenterResponse
{
    /// <summary>
    /// Cost Center ID (KOSTL).
    /// </summary>
    public string CostCenterNumber { get; set; } = string.Empty;

    /// <summary>
    /// Cost Center Name.
    /// </summary>
    public string CostCenterName { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Controlling Area.
    /// </summary>
    public string ControllingArea { get; set; } = string.Empty;

    /// <summary>
    /// Responsible Person.
    /// </summary>
    public string ResponsiblePerson { get; set; } = string.Empty;

    /// <summary>
    /// Cost Center Hierarchy.
    /// </summary>
    public string Hierarchy { get; set; } = string.Empty;

    /// <summary>
    /// Currency for cost center.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Current actual costs.
    /// </summary>
    public decimal ActualCosts { get; set; } = 0;

    /// <summary>
    /// Planned costs.
    /// </summary>
    public decimal PlannedCosts { get; set; } = 0;

    /// <summary>
    /// Valid from date.
    /// </summary>
    public DateTime ValidFrom { get; set; } = DateTime.Today;

    /// <summary>
    /// Valid to date.
    /// </summary>
    public DateTime ValidTo { get; set; } = DateTime.MaxValue;

    /// <summary>
    /// Whether the cost center is blocked.
    /// </summary>
    public bool IsBlocked { get; set; } = false;

    /// <summary>
    /// Creation date and time.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// User who created the cost center.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last change date and time.
    /// </summary>
    public DateTime LastChangedOn { get; set; }

    /// <summary>
    /// User who last changed the cost center.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;
}

/// <summary>
/// Response model for Cost Center list operations.
/// </summary>
public class CostCenterListResponse
{
    /// <summary>
    /// List of cost centers.
    /// </summary>
    public List<CostCenterResponse> CostCenters { get; set; } = new();

    /// <summary>
    /// Total count of cost centers.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; }
}

#endregion

#region Profit Center Models

/// <summary>
/// Request model for creating or updating a profit center.
/// </summary>
public class CreateProfitCenterRequest
{
    /// <summary>
    /// Profit Center ID (PRCTR).
    /// </summary>
    [Required]
    [StringLength(10)]
    public string ProfitCenterNumber { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center Name.
    /// </summary>
    [Required]
    [StringLength(50)]
    public string ProfitCenterName { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Controlling Area.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string ControllingArea { get; set; } = string.Empty;

    /// <summary>
    /// Responsible Person.
    /// </summary>
    [StringLength(12)]
    public string ResponsiblePerson { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center Group.
    /// </summary>
    [StringLength(10)]
    public string ProfitCenterGroup { get; set; } = string.Empty;

    /// <summary>
    /// Currency for profit center.
    /// </summary>
    [Required]
    [StringLength(3)]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Valid from date.
    /// </summary>
    public DateTime ValidFrom { get; set; } = DateTime.Today;

    /// <summary>
    /// Valid to date.
    /// </summary>
    public DateTime ValidTo { get; set; } = DateTime.MaxValue;
}

/// <summary>
/// Profit Center entity.
/// </summary>
public class ProfitCenter
{
    /// <summary>
    /// Profit Center ID (PRCTR).
    /// </summary>
    public string ProfitCenterNumber { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center Name.
    /// </summary>
    public string ProfitCenterName { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Controlling Area.
    /// </summary>
    public string ControllingArea { get; set; } = string.Empty;

    /// <summary>
    /// Responsible Person.
    /// </summary>
    public string ResponsiblePerson { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center Group.
    /// </summary>
    public string ProfitCenterGroup { get; set; } = string.Empty;

    /// <summary>
    /// Currency for profit center.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Current actual revenue.
    /// </summary>
    public decimal ActualRevenue { get; set; } = 0;

    /// <summary>
    /// Current actual costs.
    /// </summary>
    public decimal ActualCosts { get; set; } = 0;

    /// <summary>
    /// Planned revenue.
    /// </summary>
    public decimal PlannedRevenue { get; set; } = 0;

    /// <summary>
    /// Planned costs.
    /// </summary>
    public decimal PlannedCosts { get; set; } = 0;

    /// <summary>
    /// Valid from date.
    /// </summary>
    public DateTime ValidFrom { get; set; } = DateTime.Today;

    /// <summary>
    /// Valid to date.
    /// </summary>
    public DateTime ValidTo { get; set; } = DateTime.MaxValue;

    /// <summary>
    /// Whether the profit center is blocked.
    /// </summary>
    public bool IsBlocked { get; set; } = false;

    /// <summary>
    /// Creation date and time.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who created the profit center.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last change date and time.
    /// </summary>
    public DateTime LastChangedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who last changed the profit center.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;
}

/// <summary>
/// Response model for Profit Center operations.
/// </summary>
public class ProfitCenterResponse
{
    /// <summary>
    /// Profit Center ID (PRCTR).
    /// </summary>
    public string ProfitCenterNumber { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center Name.
    /// </summary>
    public string ProfitCenterName { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Controlling Area.
    /// </summary>
    public string ControllingArea { get; set; } = string.Empty;

    /// <summary>
    /// Responsible Person.
    /// </summary>
    public string ResponsiblePerson { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center Group.
    /// </summary>
    public string ProfitCenterGroup { get; set; } = string.Empty;

    /// <summary>
    /// Currency for profit center.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Current actual revenue.
    /// </summary>
    public decimal ActualRevenue { get; set; } = 0;

    /// <summary>
    /// Current actual costs.
    /// </summary>
    public decimal ActualCosts { get; set; } = 0;

    /// <summary>
    /// Actual profit (revenue - costs).
    /// </summary>
    public decimal ActualProfit => ActualRevenue - ActualCosts;

    /// <summary>
    /// Planned revenue.
    /// </summary>
    public decimal PlannedRevenue { get; set; } = 0;

    /// <summary>
    /// Planned costs.
    /// </summary>
    public decimal PlannedCosts { get; set; } = 0;

    /// <summary>
    /// Planned profit (planned revenue - planned costs).
    /// </summary>
    public decimal PlannedProfit => PlannedRevenue - PlannedCosts;

    /// <summary>
    /// Valid from date.
    /// </summary>
    public DateTime ValidFrom { get; set; } = DateTime.Today;

    /// <summary>
    /// Valid to date.
    /// </summary>
    public DateTime ValidTo { get; set; } = DateTime.MaxValue;

    /// <summary>
    /// Whether the profit center is blocked.
    /// </summary>
    public bool IsBlocked { get; set; } = false;

    /// <summary>
    /// Creation date and time.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// User who created the profit center.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last change date and time.
    /// </summary>
    public DateTime LastChangedOn { get; set; }

    /// <summary>
    /// User who last changed the profit center.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;
}

/// <summary>
/// Response model for Profit Center list operations.
/// </summary>
public class ProfitCenterListResponse
{
    /// <summary>
    /// List of profit centers.
    /// </summary>
    public List<ProfitCenterResponse> ProfitCenters { get; set; } = new();

    /// <summary>
    /// Total count of profit centers.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; }
}

#endregion

#region Financial Document Models

/// <summary>
/// Request model for creating a financial document.
/// </summary>
public class CreateFinancialDocumentRequest
{
    /// <summary>
    /// Document Type (SA, KR, DZ, etc.).
    /// </summary>
    [Required]
    [StringLength(2)]
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Posting Date.
    /// </summary>
    [Required]
    public DateTime PostingDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Document Date.
    /// </summary>
    [Required]
    public DateTime DocumentDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Document Header Text.
    /// </summary>
    [StringLength(100)]
    public string DocumentHeaderText { get; set; } = string.Empty;

    /// <summary>
    /// Reference Document Number.
    /// </summary>
    [StringLength(16)]
    public string ReferenceDocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Currency for the document.
    /// </summary>
    [Required]
    [StringLength(3)]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Document line items.
    /// </summary>
    public List<CreateFinancialDocumentLineRequest> LineItems { get; set; } = new();
}

/// <summary>
/// Request model for creating a financial document line item.
/// </summary>
public class CreateFinancialDocumentLineRequest
{
    /// <summary>
    /// GL Account Number.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string GLAccount { get; set; } = string.Empty;

    /// <summary>
    /// Debit Amount.
    /// </summary>
    public decimal DebitAmount { get; set; } = 0;

    /// <summary>
    /// Credit Amount.
    /// </summary>
    public decimal CreditAmount { get; set; } = 0;

    /// <summary>
    /// Cost Center.
    /// </summary>
    [StringLength(10)]
    public string CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center.
    /// </summary>
    [StringLength(10)]
    public string ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// Line item text.
    /// </summary>
    [StringLength(100)]
    public string LineItemText { get; set; } = string.Empty;

    /// <summary>
    /// Assignment field.
    /// </summary>
    [StringLength(18)]
    public string Assignment { get; set; } = string.Empty;
}

/// <summary>
/// Financial Document entity.
/// </summary>
public class FinancialDocument
{
    /// <summary>
    /// Document Number.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Document Type (SA, KR, DZ, etc.).
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Fiscal Year.
    /// </summary>
    public int FiscalYear { get; set; } = DateTime.Today.Year;

    /// <summary>
    /// Posting Date.
    /// </summary>
    public DateTime PostingDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Document Date.
    /// </summary>
    public DateTime DocumentDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Document Header Text.
    /// </summary>
    public string DocumentHeaderText { get; set; } = string.Empty;

    /// <summary>
    /// Reference Document Number.
    /// </summary>
    public string ReferenceDocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Currency for the document.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Document Status.
    /// </summary>
    public string DocumentStatus { get; set; } = "Posted";

    /// <summary>
    /// Whether the document is reversed.
    /// </summary>
    public bool IsReversed { get; set; } = false;

    /// <summary>
    /// Reversing document number.
    /// </summary>
    public string ReversingDocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Reversal date.
    /// </summary>
    public DateTime? ReversalDate { get; set; }

    /// <summary>
    /// Document line items.
    /// </summary>
    public List<FinancialDocumentLine> LineItems { get; set; } = new();

    /// <summary>
    /// Creation date and time.
    /// </summary>
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who created the document.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last change date and time.
    /// </summary>
    public DateTime LastChangedOn { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who last changed the document.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;
}

/// <summary>
/// Financial Document Line Item entity.
/// </summary>
public class FinancialDocumentLine
{
    /// <summary>
    /// Document Number.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Line Item Number.
    /// </summary>
    public int LineItemNumber { get; set; } = 0;

    /// <summary>
    /// GL Account Number.
    /// </summary>
    public string GLAccount { get; set; } = string.Empty;

    /// <summary>
    /// Debit Amount.
    /// </summary>
    public decimal DebitAmount { get; set; } = 0;

    /// <summary>
    /// Credit Amount.
    /// </summary>
    public decimal CreditAmount { get; set; } = 0;

    /// <summary>
    /// Cost Center.
    /// </summary>
    public string CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center.
    /// </summary>
    public string ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// Line item text.
    /// </summary>
    public string LineItemText { get; set; } = string.Empty;

    /// <summary>
    /// Assignment field.
    /// </summary>
    public string Assignment { get; set; } = string.Empty;

    /// <summary>
    /// Posting Key.
    /// </summary>
    public string PostingKey { get; set; } = string.Empty;

    /// <summary>
    /// Whether the line item is reversed.
    /// </summary>
    public bool IsReversed { get; set; } = false;
}

/// <summary>
/// Response model for Financial Document operations.
/// </summary>
public class FinancialDocumentResponse
{
    /// <summary>
    /// Document Number.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Document Type (SA, KR, DZ, etc.).
    /// </summary>
    public string DocumentType { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Fiscal Year.
    /// </summary>
    public int FiscalYear { get; set; } = DateTime.Today.Year;

    /// <summary>
    /// Posting Date.
    /// </summary>
    public DateTime PostingDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Document Date.
    /// </summary>
    public DateTime DocumentDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Document Header Text.
    /// </summary>
    public string DocumentHeaderText { get; set; } = string.Empty;

    /// <summary>
    /// Reference Document Number.
    /// </summary>
    public string ReferenceDocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Currency for the document.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Document Status.
    /// </summary>
    public string DocumentStatus { get; set; } = "Posted";

    /// <summary>
    /// Whether the document is reversed.
    /// </summary>
    public bool IsReversed { get; set; } = false;

    /// <summary>
    /// Reversing document number.
    /// </summary>
    public string ReversingDocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Reversal date.
    /// </summary>
    public DateTime? ReversalDate { get; set; }

    /// <summary>
    /// Document line items.
    /// </summary>
    public List<FinancialDocumentLineResponse> LineItems { get; set; } = new();

    /// <summary>
    /// Creation date and time.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// User who created the document.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Last change date and time.
    /// </summary>
    public DateTime LastChangedOn { get; set; }

    /// <summary>
    /// User who last changed the document.
    /// </summary>
    public string LastChangedBy { get; set; } = string.Empty;
}

/// <summary>
/// Response model for Financial Document Line Item operations.
/// </summary>
public class FinancialDocumentLineResponse
{
    /// <summary>
    /// Document Number.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Line Item Number.
    /// </summary>
    public int LineItemNumber { get; set; } = 0;

    /// <summary>
    /// GL Account Number.
    /// </summary>
    public string GLAccount { get; set; } = string.Empty;

    /// <summary>
    /// Debit Amount.
    /// </summary>
    public decimal DebitAmount { get; set; } = 0;

    /// <summary>
    /// Credit Amount.
    /// </summary>
    public decimal CreditAmount { get; set; } = 0;

    /// <summary>
    /// Cost Center.
    /// </summary>
    public string CostCenter { get; set; } = string.Empty;

    /// <summary>
    /// Profit Center.
    /// </summary>
    public string ProfitCenter { get; set; } = string.Empty;

    /// <summary>
    /// Line item text.
    /// </summary>
    public string LineItemText { get; set; } = string.Empty;

    /// <summary>
    /// Assignment field.
    /// </summary>
    public string Assignment { get; set; } = string.Empty;

    /// <summary>
    /// Posting Key.
    /// </summary>
    public string PostingKey { get; set; } = string.Empty;

    /// <summary>
    /// Whether the line item is reversed.
    /// </summary>
    public bool IsReversed { get; set; } = false;
}

/// <summary>
/// Response model for Financial Document list operations.
/// </summary>
public class FinancialDocumentListResponse
{
    /// <summary>
    /// List of financial documents.
    /// </summary>
    public List<FinancialDocumentResponse> Documents { get; set; } = new();

    /// <summary>
    /// Total count of documents.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Page size.
    /// </summary>
    public int PageSize { get; set; }
}

#endregion

#region Posting Simulation Models

/// <summary>
/// Request model for posting simulation.
/// </summary>
public class PostingSimulationRequest
{
    /// <summary>
    /// Document data to simulate.
    /// </summary>
    public CreateFinancialDocumentRequest DocumentData { get; set; } = new();

    /// <summary>
    /// Whether to check for balance validation.
    /// </summary>
    public bool CheckBalance { get; set; } = true;

    /// <summary>
    /// Whether to check for account validation.
    /// </summary>
    public bool CheckAccountValidation { get; set; } = true;

    /// <summary>
    /// Whether to check for authorization.
    /// </summary>
    public bool CheckAuthorization { get; set; } = true;
}

/// <summary>
/// Response model for posting simulation.
/// </summary>
public class PostingSimulationResponse
{
    /// <summary>
    /// Whether the posting simulation was successful.
    /// </summary>
    public bool IsSuccessful { get; set; } = true;

    /// <summary>
    /// Simulated document number.
    /// </summary>
    public string SimulatedDocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Validation messages.
    /// </summary>
    public List<PostingValidationMessage> ValidationMessages { get; set; } = new();

    /// <summary>
    /// Account balance changes.
    /// </summary>
    public List<BalanceChangeItem> BalanceChanges { get; set; } = new();

    /// <summary>
    /// Simulated document data.
    /// </summary>
    public FinancialDocumentResponse? SimulatedDocument { get; set; }
}

/// <summary>
/// Posting validation message.
/// </summary>
public class PostingValidationMessage
{
    /// <summary>
    /// Message type (Error, Warning, Info).
    /// </summary>
    public string MessageType { get; set; } = string.Empty;

    /// <summary>
    /// Message code.
    /// </summary>
    public string MessageCode { get; set; } = string.Empty;

    /// <summary>
    /// Message text.
    /// </summary>
    public string MessageText { get; set; } = string.Empty;

    /// <summary>
    /// GL Account related to the message.
    /// </summary>
    public string GLAccount { get; set; } = string.Empty;

    /// <summary>
    /// Line item number related to the message.
    /// </summary>
    public int LineItemNumber { get; set; } = 0;
}

/// <summary>
/// Balance change item for posting simulation.
/// </summary>
public class BalanceChangeItem
{
    /// <summary>
    /// GL Account Number.
    /// </summary>
    public string GLAccount { get; set; } = string.Empty;

    /// <summary>
    /// Account Name.
    /// </summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Current balance before posting.
    /// </summary>
    public decimal CurrentBalance { get; set; } = 0;

    /// <summary>
    /// Change amount (positive for increase, negative for decrease).
    /// </summary>
    public decimal ChangeAmount { get; set; } = 0;

    /// <summary>
    /// New balance after posting.
    /// </summary>
    public decimal NewBalance { get; set; } = 0;

    /// <summary>
    /// Currency.
    /// </summary>
    public string Currency { get; set; } = "USD";
}

#endregion

#region Document Reversal Models

/// <summary>
/// Request model for document reversal.
/// </summary>
public class DocumentReversalRequest
{
    /// <summary>
    /// Document number to reverse.
    /// </summary>
    [Required]
    [StringLength(10)]
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    [Required]
    [StringLength(4)]
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Fiscal Year.
    /// </summary>
    [Required]
    public int FiscalYear { get; set; } = DateTime.Today.Year;

    /// <summary>
    /// Reversal date.
    /// </summary>
    [Required]
    public DateTime ReversalDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Reversal reason.
    /// </summary>
    [StringLength(2)]
    public string ReversalReason { get; set; } = string.Empty;

    /// <summary>
    /// Reversal text.
    /// </summary>
    [StringLength(100)]
    public string ReversalText { get; set; } = string.Empty;
}

/// <summary>
/// Response model for document reversal.
/// </summary>
public class DocumentReversalResponse
{
    /// <summary>
    /// Original document number.
    /// </summary>
    public string OriginalDocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Reversing document number.
    /// </summary>
    public string ReversingDocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Company Code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Fiscal Year.
    /// </summary>
    public int FiscalYear { get; set; } = DateTime.Today.Year;

    /// <summary>
    /// Reversal date.
    /// </summary>
    public DateTime ReversalDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Reversal reason.
    /// </summary>
    public string ReversalReason { get; set; } = string.Empty;

    /// <summary>
    /// Reversal text.
    /// </summary>
    public string ReversalText { get; set; } = string.Empty;

    /// <summary>
    /// Whether the reversal was successful.
    /// </summary>
    public bool IsSuccessful { get; set; } = true;

    /// <summary>
    /// Reversal messages.
    /// </summary>
    public List<string> Messages { get; set; } = new();

    /// <summary>
    /// Account balance changes from reversal.
    /// </summary>
    public List<BalanceChangeItem> BalanceChanges { get; set; } = new();
}

#endregion

#region Common Models

/// <summary>
/// SAP-formatted error response for Finance operations.
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
    /// Message type (E = Error, W = Warning, I = Info).
    /// </summary>
    public string Type { get; set; } = "E";

    /// <summary>
    /// Message class.
    /// </summary>
    public string MessageClass { get; set; } = "FI";

    /// <summary>
    /// Message number.
    /// </summary>
    public string MessageNumber { get; set; } = string.Empty;

    /// <summary>
    /// Message variables.
    /// </summary>
    public List<string> MessageVariables { get; set; } = new();
}

#endregion