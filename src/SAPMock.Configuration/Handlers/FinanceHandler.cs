using System.ComponentModel.DataAnnotations;
using SAPMock.Configuration.Models.Finance;
using SAPMock.Core;

namespace SAPMock.Configuration.Handlers;

/// <summary>
/// Handler for SAP Finance (FI) module endpoints.
/// Provides CRUD operations for GL accounts, cost centers, profit centers, and financial documents.
/// </summary>
public class FinanceHandler : ISAPModuleHandler
{
    private readonly IMockDataProvider _mockDataProvider;
    private readonly string _systemId;
    private readonly List<ISAPEndpoint> _endpoints;

    /// <summary>
    /// Initializes a new instance of the FinanceHandler.
    /// </summary>
    /// <param name="mockDataProvider">Mock data provider for data storage and retrieval.</param>
    /// <param name="systemId">The SAP system ID this handler is associated with.</param>
    public FinanceHandler(IMockDataProvider mockDataProvider, string systemId)
    {
        _mockDataProvider = mockDataProvider ?? throw new ArgumentNullException(nameof(mockDataProvider));
        _systemId = systemId ?? throw new ArgumentNullException(nameof(systemId));
        _endpoints = InitializeEndpoints();
    }

    /// <summary>
    /// Gets all endpoints for the Finance module.
    /// </summary>
    /// <param name="systemId">The SAP system ID.</param>
    /// <returns>Collection of FI module endpoints.</returns>
    public IEnumerable<ISAPEndpoint> GetEndpoints(string systemId)
    {
        return _endpoints;
    }

    /// <summary>
    /// Initializes the Finance module endpoints.
    /// </summary>
    /// <returns>List of configured endpoints.</returns>
    private List<ISAPEndpoint> InitializeEndpoints()
    {
        return new List<ISAPEndpoint>
        {
            #region GL Account Endpoints
            // GET /gl-accounts/{id}
            new SAPEndpoint
            {
                Path = "/gl-accounts/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(GLAccountResponse),
                Handler = GetGLAccountHandler,
                ErrorSimulations = new List<ErrorSimulationConfig>
                {
                    new ErrorSimulationConfig
                    {
                        ErrorType = ErrorType.Timeout,
                        Probability = 0.01,
                        DelayMs = 3000,
                        CustomMessage = "GL account service timeout",
                        SAPErrorCode = "FI_TIMEOUT"
                    }
                }
            },
            // GET /gl-accounts
            new SAPEndpoint
            {
                Path = "/gl-accounts",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(GLAccountListResponse),
                Handler = ListGLAccountsHandler
            },
            // POST /gl-accounts
            new SAPEndpoint
            {
                Path = "/gl-accounts",
                Method = "POST",
                RequestType = typeof(CreateGLAccountRequest),
                ResponseType = typeof(GLAccountResponse),
                Handler = CreateGLAccountHandler
            },
            #endregion

            #region Cost Center Endpoints
            // GET /cost-centers/{id}
            new SAPEndpoint
            {
                Path = "/cost-centers/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(CostCenterResponse),
                Handler = GetCostCenterHandler
            },
            // GET /cost-centers
            new SAPEndpoint
            {
                Path = "/cost-centers",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(CostCenterListResponse),
                Handler = ListCostCentersHandler
            },
            // POST /cost-centers
            new SAPEndpoint
            {
                Path = "/cost-centers",
                Method = "POST",
                RequestType = typeof(CreateCostCenterRequest),
                ResponseType = typeof(CostCenterResponse),
                Handler = CreateCostCenterHandler
            },
            #endregion

            #region Profit Center Endpoints
            // GET /profit-centers/{id}
            new SAPEndpoint
            {
                Path = "/profit-centers/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(ProfitCenterResponse),
                Handler = GetProfitCenterHandler
            },
            // GET /profit-centers
            new SAPEndpoint
            {
                Path = "/profit-centers",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(ProfitCenterListResponse),
                Handler = ListProfitCentersHandler
            },
            // POST /profit-centers
            new SAPEndpoint
            {
                Path = "/profit-centers",
                Method = "POST",
                RequestType = typeof(CreateProfitCenterRequest),
                ResponseType = typeof(ProfitCenterResponse),
                Handler = CreateProfitCenterHandler
            },
            #endregion

            #region Financial Document Endpoints
            // GET /documents/{id}
            new SAPEndpoint
            {
                Path = "/documents/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(FinancialDocumentResponse),
                Handler = GetFinancialDocumentHandler
            },
            // GET /documents
            new SAPEndpoint
            {
                Path = "/documents",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(FinancialDocumentListResponse),
                Handler = ListFinancialDocumentsHandler
            },
            // POST /documents
            new SAPEndpoint
            {
                Path = "/documents",
                Method = "POST",
                RequestType = typeof(CreateFinancialDocumentRequest),
                ResponseType = typeof(FinancialDocumentResponse),
                Handler = CreateFinancialDocumentHandler
            },
            #endregion

            #region Posting Simulation Endpoints
            // POST /posting-simulation
            new SAPEndpoint
            {
                Path = "/posting-simulation",
                Method = "POST",
                RequestType = typeof(PostingSimulationRequest),
                ResponseType = typeof(PostingSimulationResponse),
                Handler = PostingSimulationHandler
            },
            #endregion

            #region Document Reversal Endpoints
            // POST /document-reversal
            new SAPEndpoint
            {
                Path = "/document-reversal",
                Method = "POST",
                RequestType = typeof(DocumentReversalRequest),
                ResponseType = typeof(DocumentReversalResponse),
                Handler = DocumentReversalHandler
            },
            #endregion

            #region Balance Calculation Endpoints
            // GET /balance-calculation/{accountNumber}
            new SAPEndpoint
            {
                Path = "/balance-calculation/{accountNumber}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(object),
                Handler = BalanceCalculationHandler
            },
            #endregion
        };
    }

    #region GL Account Operations

    /// <summary>
    /// Retrieves a specific GL account by its number.
    /// </summary>
    public async Task<object> GetGLAccountAsync(string accountNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return CreateErrorResponse("FI001", "Account number is required", "Account number parameter cannot be empty");
            }

            var allAccounts = await GetAllGLAccountsAsync();
            var account = allAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                return CreateErrorResponse("FI002", "Account not found", $"GL account {accountNumber} does not exist");
            }

            return MapGLAccountToResponse(account);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("FI999", "Internal error", $"Error retrieving GL account: {ex.Message}");
        }
    }

    /// <summary>
    /// Lists GL accounts with pagination support.
    /// </summary>
    public async Task<object> ListGLAccountsAsync(int page = 1, int pageSize = 50, string? accountType = null, string? companyCode = null)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 50;

            var allAccounts = await GetAllGLAccountsAsync();
            var filteredAccounts = allAccounts.Where(a => !a.IsMarkedForDeletion);

            if (!string.IsNullOrWhiteSpace(accountType))
            {
                filteredAccounts = filteredAccounts.Where(a => a.AccountType.Equals(accountType, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(companyCode))
            {
                filteredAccounts = filteredAccounts.Where(a => a.CompanyCode.Equals(companyCode, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = filteredAccounts.Count();
            var accounts = filteredAccounts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapGLAccountToResponse)
                .ToList();

            return new GLAccountListResponse
            {
                Accounts = accounts,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("FI999", "Internal error", $"Error listing GL accounts: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new GL account.
    /// </summary>
    public async Task<object> CreateGLAccountAsync(CreateGLAccountRequest request)
    {
        try
        {
            var validationResult = ValidateCreateGLAccountRequest(request);
            if (validationResult != null)
                return validationResult;

            var allAccounts = await GetAllGLAccountsAsync();
            var existingAccount = allAccounts.FirstOrDefault(a => a.AccountNumber == request.AccountNumber);
            
            if (existingAccount != null)
            {
                return CreateErrorResponse("FI003", "Account already exists", $"GL account {request.AccountNumber} already exists");
            }

            var account = new GLAccount
            {
                AccountNumber = request.AccountNumber,
                AccountName = request.AccountName,
                AccountType = request.AccountType,
                AccountGroup = request.AccountGroup,
                CompanyCode = request.CompanyCode,
                Currency = request.Currency,
                IsBlocked = request.IsBlocked,
                IsMarkedForDeletion = request.IsMarkedForDeletion,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "SYSTEM",
                LastChangedOn = DateTime.UtcNow,
                LastChangedBy = "SYSTEM"
            };

            var accountList = allAccounts.ToList();
            accountList.Add(account);
            await SaveAllGLAccountsAsync(accountList);

            return MapGLAccountToResponse(account);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("FI999", "Internal error", $"Error creating GL account: {ex.Message}");
        }
    }

    #endregion

    #region Posting Simulation

    /// <summary>
    /// Simulates financial document posting.
    /// </summary>
    public async Task<object> PostingSimulationAsync(PostingSimulationRequest request)
    {
        try
        {
            var response = new PostingSimulationResponse
            {
                IsSuccessful = true,
                SimulatedDocumentNumber = GenerateDocumentNumber(),
                ValidationMessages = new List<PostingValidationMessage>(),
                BalanceChanges = new List<BalanceChangeItem>()
            };

            // Validate document balance
            if (request.CheckBalance)
            {
                var totalDebits = request.DocumentData.LineItems.Sum(l => l.DebitAmount);
                var totalCredits = request.DocumentData.LineItems.Sum(l => l.CreditAmount);
                
                if (Math.Abs(totalDebits - totalCredits) > 0.01m)
                {
                    response.IsSuccessful = false;
                    response.ValidationMessages.Add(new PostingValidationMessage
                    {
                        MessageType = "Error",
                        MessageCode = "FI004",
                        MessageText = "Document is not balanced",
                        GLAccount = "",
                        LineItemNumber = 0
                    });
                }
            }

            // Validate GL accounts
            if (request.CheckAccountValidation)
            {
                var allAccounts = await GetAllGLAccountsAsync();
                for (int i = 0; i < request.DocumentData.LineItems.Count; i++)
                {
                    var lineItem = request.DocumentData.LineItems[i];
                    var account = allAccounts.FirstOrDefault(a => a.AccountNumber == lineItem.GLAccount);
                    
                    if (account == null)
                    {
                        response.IsSuccessful = false;
                        response.ValidationMessages.Add(new PostingValidationMessage
                        {
                            MessageType = "Error",
                            MessageCode = "FI005",
                            MessageText = $"GL account {lineItem.GLAccount} does not exist",
                            GLAccount = lineItem.GLAccount,
                            LineItemNumber = i + 1
                        });
                    }
                    else if (account.IsBlocked)
                    {
                        response.IsSuccessful = false;
                        response.ValidationMessages.Add(new PostingValidationMessage
                        {
                            MessageType = "Error",
                            MessageCode = "FI006",
                            MessageText = $"GL account {lineItem.GLAccount} is blocked for postings",
                            GLAccount = lineItem.GLAccount,
                            LineItemNumber = i + 1
                        });
                    }
                    else
                    {
                        // Calculate balance changes
                        var changeAmount = lineItem.DebitAmount - lineItem.CreditAmount;
                        var newBalance = account.Balance + changeAmount;
                        
                        response.BalanceChanges.Add(new BalanceChangeItem
                        {
                            GLAccount = account.AccountNumber,
                            AccountName = account.AccountName,
                            CurrentBalance = account.Balance,
                            ChangeAmount = changeAmount,
                            NewBalance = newBalance,
                            Currency = account.Currency
                        });
                    }
                }
            }

            // Create simulated document if successful
            if (response.IsSuccessful)
            {
                var simulatedDocument = new FinancialDocument
                {
                    DocumentNumber = response.SimulatedDocumentNumber,
                    DocumentType = request.DocumentData.DocumentType,
                    CompanyCode = request.DocumentData.CompanyCode,
                    PostingDate = request.DocumentData.PostingDate,
                    DocumentDate = request.DocumentData.DocumentDate,
                    DocumentHeaderText = request.DocumentData.DocumentHeaderText,
                    ReferenceDocumentNumber = request.DocumentData.ReferenceDocumentNumber,
                    Currency = request.DocumentData.Currency,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = "SYSTEM",
                    LastChangedOn = DateTime.UtcNow,
                    LastChangedBy = "SYSTEM"
                };

                // Add line items
                for (int i = 0; i < request.DocumentData.LineItems.Count; i++)
                {
                    var lineRequest = request.DocumentData.LineItems[i];
                    simulatedDocument.LineItems.Add(new FinancialDocumentLine
                    {
                        DocumentNumber = response.SimulatedDocumentNumber,
                        LineItemNumber = i + 1,
                        GLAccount = lineRequest.GLAccount,
                        DebitAmount = lineRequest.DebitAmount,
                        CreditAmount = lineRequest.CreditAmount,
                        CostCenter = lineRequest.CostCenter,
                        ProfitCenter = lineRequest.ProfitCenter,
                        LineItemText = lineRequest.LineItemText,
                        Assignment = lineRequest.Assignment,
                        PostingKey = lineRequest.DebitAmount > 0 ? "40" : "50" // Simple posting key logic
                    });
                }

                response.SimulatedDocument = MapFinancialDocumentToResponse(simulatedDocument);
            }

            return response;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("FI999", "Internal error", $"Error in posting simulation: {ex.Message}");
        }
    }

    #endregion

    #region Document Reversal

    /// <summary>
    /// Reverses a financial document.
    /// </summary>
    public async Task<object> DocumentReversalAsync(DocumentReversalRequest request)
    {
        try
        {
            var allDocuments = await GetAllFinancialDocumentsAsync();
            var originalDocument = allDocuments.FirstOrDefault(d => 
                d.DocumentNumber == request.DocumentNumber && 
                d.CompanyCode == request.CompanyCode && 
                d.FiscalYear == request.FiscalYear);

            if (originalDocument == null)
            {
                return CreateErrorResponse("FI007", "Document not found", 
                    $"Document {request.DocumentNumber} not found in company {request.CompanyCode} for fiscal year {request.FiscalYear}");
            }

            if (originalDocument.IsReversed)
            {
                return CreateErrorResponse("FI008", "Document already reversed", 
                    $"Document {request.DocumentNumber} is already reversed");
            }

            var reversingDocumentNumber = GenerateDocumentNumber();
            
            // Create reversing document
            var reversingDocument = new FinancialDocument
            {
                DocumentNumber = reversingDocumentNumber,
                DocumentType = originalDocument.DocumentType,
                CompanyCode = originalDocument.CompanyCode,
                FiscalYear = request.FiscalYear,
                PostingDate = request.ReversalDate,
                DocumentDate = request.ReversalDate,
                DocumentHeaderText = $"Reversal of {originalDocument.DocumentNumber} - {request.ReversalText}",
                ReferenceDocumentNumber = originalDocument.DocumentNumber,
                Currency = originalDocument.Currency,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "SYSTEM",
                LastChangedOn = DateTime.UtcNow,
                LastChangedBy = "SYSTEM"
            };

            // Create reversing line items (with opposite signs)
            foreach (var originalLine in originalDocument.LineItems)
            {
                reversingDocument.LineItems.Add(new FinancialDocumentLine
                {
                    DocumentNumber = reversingDocumentNumber,
                    LineItemNumber = originalLine.LineItemNumber,
                    GLAccount = originalLine.GLAccount,
                    DebitAmount = originalLine.CreditAmount, // Swap debit and credit
                    CreditAmount = originalLine.DebitAmount,
                    CostCenter = originalLine.CostCenter,
                    ProfitCenter = originalLine.ProfitCenter,
                    LineItemText = $"Reversal: {originalLine.LineItemText}",
                    Assignment = originalLine.Assignment,
                    PostingKey = originalLine.PostingKey
                });
            }

            // Update original document
            originalDocument.IsReversed = true;
            originalDocument.ReversingDocumentNumber = reversingDocumentNumber;
            originalDocument.ReversalDate = request.ReversalDate;
            originalDocument.LastChangedOn = DateTime.UtcNow;
            originalDocument.LastChangedBy = "SYSTEM";

            // Save both documents
            var documentList = allDocuments.ToList();
            documentList.Add(reversingDocument);
            await SaveAllFinancialDocumentsAsync(documentList);

            // Calculate balance changes
            var balanceChanges = new List<BalanceChangeItem>();
            var allAccounts = await GetAllGLAccountsAsync();
            
            foreach (var line in reversingDocument.LineItems)
            {
                var account = allAccounts.FirstOrDefault(a => a.AccountNumber == line.GLAccount);
                if (account != null)
                {
                    var changeAmount = line.DebitAmount - line.CreditAmount;
                    balanceChanges.Add(new BalanceChangeItem
                    {
                        GLAccount = account.AccountNumber,
                        AccountName = account.AccountName,
                        CurrentBalance = account.Balance,
                        ChangeAmount = changeAmount,
                        NewBalance = account.Balance + changeAmount,
                        Currency = account.Currency
                    });
                }
            }

            return new DocumentReversalResponse
            {
                OriginalDocumentNumber = request.DocumentNumber,
                ReversingDocumentNumber = reversingDocumentNumber,
                CompanyCode = request.CompanyCode,
                FiscalYear = request.FiscalYear,
                ReversalDate = request.ReversalDate,
                ReversalReason = request.ReversalReason,
                ReversalText = request.ReversalText,
                IsSuccessful = true,
                Messages = new List<string> { "Document reversed successfully" },
                BalanceChanges = balanceChanges
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("FI999", "Internal error", $"Error reversing document: {ex.Message}");
        }
    }

    #endregion

    #region Balance Calculation

    /// <summary>
    /// Calculates current balance for a GL account.
    /// </summary>
    public async Task<object> CalculateAccountBalanceAsync(string accountNumber, int? fiscalYear = null, int? period = null)
    {
        try
        {
            var allAccounts = await GetAllGLAccountsAsync();
            var account = allAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                return CreateErrorResponse("FI002", "Account not found", $"GL account {accountNumber} does not exist");
            }

            // For simplicity, return current balance
            // In a real system, this would calculate based on fiscal year/period
            return new
            {
                AccountNumber = account.AccountNumber,
                AccountName = account.AccountName,
                CurrentBalance = account.Balance,
                Currency = account.Currency,
                FiscalYear = fiscalYear ?? DateTime.Today.Year,
                Period = period ?? DateTime.Today.Month,
                CalculatedOn = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("FI999", "Internal error", $"Error calculating balance: {ex.Message}");
        }
    }

    #endregion

    #region Endpoint Handlers

    private async Task<object> GetGLAccountHandler(object request)
    {
        var accountNumber = ExtractIdFromRequest(request);
        return await GetGLAccountAsync(accountNumber);
    }

    private async Task<object> ListGLAccountsHandler(object request)
    {
        var (page, pageSize, accountType, companyCode) = ExtractGLAccountListParametersFromRequest(request);
        return await ListGLAccountsAsync(page, pageSize, accountType, companyCode);
    }

    private async Task<object> CreateGLAccountHandler(object request)
    {
        if (request is CreateGLAccountRequest createRequest)
        {
            return await CreateGLAccountAsync(createRequest);
        }
        return CreateErrorResponse("FI009", "Invalid request", "Invalid request format for GL account creation");
    }

    private async Task<object> GetCostCenterHandler(object request)
    {
        return CreateErrorResponse("FI010", "Not implemented", "Cost center operations not implemented yet");
    }

    private async Task<object> ListCostCentersHandler(object request)
    {
        return CreateErrorResponse("FI010", "Not implemented", "Cost center operations not implemented yet");
    }

    private async Task<object> CreateCostCenterHandler(object request)
    {
        return CreateErrorResponse("FI010", "Not implemented", "Cost center operations not implemented yet");
    }

    private async Task<object> GetProfitCenterHandler(object request)
    {
        return CreateErrorResponse("FI011", "Not implemented", "Profit center operations not implemented yet");
    }

    private async Task<object> ListProfitCentersHandler(object request)
    {
        return CreateErrorResponse("FI011", "Not implemented", "Profit center operations not implemented yet");
    }

    private async Task<object> CreateProfitCenterHandler(object request)
    {
        return CreateErrorResponse("FI011", "Not implemented", "Profit center operations not implemented yet");
    }

    private async Task<object> GetFinancialDocumentHandler(object request)
    {
        return CreateErrorResponse("FI012", "Not implemented", "Financial document retrieval not implemented yet");
    }

    private async Task<object> ListFinancialDocumentsHandler(object request)
    {
        return CreateErrorResponse("FI012", "Not implemented", "Financial document listing not implemented yet");
    }

    private async Task<object> CreateFinancialDocumentHandler(object request)
    {
        return CreateErrorResponse("FI012", "Not implemented", "Financial document creation not implemented yet");
    }

    private async Task<object> PostingSimulationHandler(object request)
    {
        if (request is PostingSimulationRequest simulationRequest)
        {
            return await PostingSimulationAsync(simulationRequest);
        }
        return CreateErrorResponse("FI013", "Invalid request", "Invalid request format for posting simulation");
    }

    private async Task<object> DocumentReversalHandler(object request)
    {
        if (request is DocumentReversalRequest reversalRequest)
        {
            return await DocumentReversalAsync(reversalRequest);
        }
        return CreateErrorResponse("FI014", "Invalid request", "Invalid request format for document reversal");
    }

    private async Task<object> BalanceCalculationHandler(object request)
    {
        var accountNumber = ExtractIdFromRequest(request);
        return await CalculateAccountBalanceAsync(accountNumber);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets all GL accounts from the collection.
    /// </summary>
    private async Task<IEnumerable<GLAccount>> GetAllGLAccountsAsync()
    {
        try
        {
            var wrapper = await _mockDataProvider.GetDataAsync<GLAccountsCollectionWrapper>("default/default/glaccountscollectionwrapper");
            return wrapper.Accounts;
        }
        catch (Exception)
        {
            return new List<GLAccount>();
        }
    }

    /// <summary>
    /// Saves all GL accounts to the collection.
    /// </summary>
    private async Task SaveAllGLAccountsAsync(IEnumerable<GLAccount> accounts)
    {
        var wrapper = new GLAccountsCollectionWrapper { Accounts = accounts.ToList() };
        await _mockDataProvider.SaveDataAsync(wrapper);
    }

    /// <summary>
    /// Gets all financial documents from the collection.
    /// </summary>
    private async Task<IEnumerable<FinancialDocument>> GetAllFinancialDocumentsAsync()
    {
        try
        {
            var wrapper = await _mockDataProvider.GetDataAsync<FinancialDocumentsCollectionWrapper>("default/default/financialdocumentscollectionwrapper");
            return wrapper.Documents;
        }
        catch (Exception)
        {
            return new List<FinancialDocument>();
        }
    }

    /// <summary>
    /// Saves all financial documents to the collection.
    /// </summary>
    private async Task SaveAllFinancialDocumentsAsync(IEnumerable<FinancialDocument> documents)
    {
        var wrapper = new FinancialDocumentsCollectionWrapper { Documents = documents.ToList() };
        await _mockDataProvider.SaveDataAsync(wrapper);
    }

    /// <summary>
    /// Wrapper class for GL accounts collection.
    /// </summary>
    private class GLAccountsCollectionWrapper
    {
        public List<GLAccount> Accounts { get; set; } = new();
    }

    /// <summary>
    /// Wrapper class for financial documents collection.
    /// </summary>
    private class FinancialDocumentsCollectionWrapper
    {
        public List<FinancialDocument> Documents { get; set; } = new();
    }

    /// <summary>
    /// Maps a GLAccount entity to a GLAccountResponse.
    /// </summary>
    private static GLAccountResponse MapGLAccountToResponse(GLAccount account)
    {
        return new GLAccountResponse
        {
            AccountNumber = account.AccountNumber,
            AccountName = account.AccountName,
            AccountType = account.AccountType,
            AccountGroup = account.AccountGroup,
            CompanyCode = account.CompanyCode,
            Currency = account.Currency,
            Balance = account.Balance,
            IsBlocked = account.IsBlocked,
            IsMarkedForDeletion = account.IsMarkedForDeletion,
            CreatedOn = account.CreatedOn,
            CreatedBy = account.CreatedBy,
            LastChangedOn = account.LastChangedOn,
            LastChangedBy = account.LastChangedBy
        };
    }

    /// <summary>
    /// Maps a FinancialDocument entity to a FinancialDocumentResponse.
    /// </summary>
    private static FinancialDocumentResponse MapFinancialDocumentToResponse(FinancialDocument document)
    {
        return new FinancialDocumentResponse
        {
            DocumentNumber = document.DocumentNumber,
            DocumentType = document.DocumentType,
            CompanyCode = document.CompanyCode,
            FiscalYear = document.FiscalYear,
            PostingDate = document.PostingDate,
            DocumentDate = document.DocumentDate,
            DocumentHeaderText = document.DocumentHeaderText,
            ReferenceDocumentNumber = document.ReferenceDocumentNumber,
            Currency = document.Currency,
            DocumentStatus = document.DocumentStatus,
            IsReversed = document.IsReversed,
            ReversingDocumentNumber = document.ReversingDocumentNumber,
            ReversalDate = document.ReversalDate,
            LineItems = document.LineItems.Select(MapFinancialDocumentLineToResponse).ToList(),
            CreatedOn = document.CreatedOn,
            CreatedBy = document.CreatedBy,
            LastChangedOn = document.LastChangedOn,
            LastChangedBy = document.LastChangedBy
        };
    }

    /// <summary>
    /// Maps a FinancialDocumentLine entity to a FinancialDocumentLineResponse.
    /// </summary>
    private static FinancialDocumentLineResponse MapFinancialDocumentLineToResponse(FinancialDocumentLine line)
    {
        return new FinancialDocumentLineResponse
        {
            DocumentNumber = line.DocumentNumber,
            LineItemNumber = line.LineItemNumber,
            GLAccount = line.GLAccount,
            DebitAmount = line.DebitAmount,
            CreditAmount = line.CreditAmount,
            CostCenter = line.CostCenter,
            ProfitCenter = line.ProfitCenter,
            LineItemText = line.LineItemText,
            Assignment = line.Assignment,
            PostingKey = line.PostingKey,
            IsReversed = line.IsReversed
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
            MessageClass = "FI",
            MessageNumber = errorCode,
            MessageVariables = new List<string>()
        };
    }

    /// <summary>
    /// Validates the create GL account request.
    /// </summary>
    private static SAPErrorResponse? ValidateCreateGLAccountRequest(CreateGLAccountRequest request)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(request, context, results, true))
        {
            var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            return new SAPErrorResponse
            {
                ErrorCode = "FI015",
                Message = "Validation failed",
                Details = errors,
                Type = "E",
                MessageClass = "FI",
                MessageNumber = "015"
            };
        }

        return null;
    }

    /// <summary>
    /// Generates a new document number.
    /// </summary>
    private static string GenerateDocumentNumber()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(100, 999);
        return $"DOC{timestamp}{random}";
    }

    /// <summary>
    /// Extracts ID from request context.
    /// </summary>
    private static string ExtractIdFromRequest(object request)
    {
        if (request is not null)
        {
            var type = request.GetType();
            var routeParametersProperty = type.GetProperty("RouteParameters");
            if (routeParametersProperty != null)
            {
                var routeParams = routeParametersProperty.GetValue(request) as Dictionary<string, object?>;
                if (routeParams != null && routeParams.ContainsKey("id"))
                {
                    return routeParams["id"]?.ToString() ?? "DEFAULT_ID";
                }
            }
        }
        return "DEFAULT_ID";
    }

    /// <summary>
    /// Extracts GL account list parameters from request context.
    /// </summary>
    private static (int page, int pageSize, string? accountType, string? companyCode) ExtractGLAccountListParametersFromRequest(object request)
    {
        if (request is not null)
        {
            var type = request.GetType();
            var queryParametersProperty = type.GetProperty("QueryParameters");
            if (queryParametersProperty != null)
            {
                var queryParams = queryParametersProperty.GetValue(request) as Dictionary<string, string>;
                if (queryParams != null)
                {
                    int.TryParse(queryParams.GetValueOrDefault("page", "1"), out int page);
                    int.TryParse(queryParams.GetValueOrDefault("pageSize", "50"), out int pageSize);
                    var accountType = queryParams.GetValueOrDefault("accountType");
                    var companyCode = queryParams.GetValueOrDefault("companyCode");
                    
                    return (page, pageSize, accountType, companyCode);
                }
            }
        }
        return (1, 50, null, null);
    }

    #endregion
}