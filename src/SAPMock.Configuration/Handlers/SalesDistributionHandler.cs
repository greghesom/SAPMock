using System.ComponentModel.DataAnnotations;
using SAPMock.Configuration.Models.SalesDistribution;
using SAPMock.Core;

namespace SAPMock.Configuration.Handlers;

/// <summary>
/// Handler for SAP Sales & Distribution (SD) module endpoints.
/// Provides CRUD operations for customers, sales orders, deliveries, and invoices using the mock data provider.
/// </summary>
public class SalesDistributionHandler : ISAPModuleHandler
{
    private readonly IMockDataProvider _mockDataProvider;
    private readonly string _systemId;
    private readonly List<ISAPEndpoint> _endpoints;

    /// <summary>
    /// Initializes a new instance of the SalesDistributionHandler.
    /// </summary>
    /// <param name="mockDataProvider">Mock data provider for data storage and retrieval.</param>
    /// <param name="systemId">The SAP system ID this handler is associated with.</param>
    public SalesDistributionHandler(IMockDataProvider mockDataProvider, string systemId)
    {
        _mockDataProvider = mockDataProvider ?? throw new ArgumentNullException(nameof(mockDataProvider));
        _systemId = systemId ?? throw new ArgumentNullException(nameof(systemId));
        _endpoints = InitializeEndpoints();
    }

    /// <summary>
    /// Gets all endpoints for the Sales & Distribution module.
    /// </summary>
    /// <param name="systemId">The SAP system ID.</param>
    /// <returns>Collection of SD module endpoints.</returns>
    public IEnumerable<ISAPEndpoint> GetEndpoints(string systemId)
    {
        return _endpoints;
    }

    /// <summary>
    /// Initializes the Sales & Distribution module endpoints.
    /// </summary>
    /// <returns>List of configured endpoints.</returns>
    private List<ISAPEndpoint> InitializeEndpoints()
    {
        return new List<ISAPEndpoint>
        {
            #region Customer Endpoints
            // GET /customers/{id}
            new SAPEndpoint
            {
                Path = "/customers/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(CustomerResponse),
                Handler = GetCustomerHandler
            },
            // GET /customers
            new SAPEndpoint
            {
                Path = "/customers",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(CustomerListResponse),
                Handler = ListCustomersHandler
            },
            // POST /customers
            new SAPEndpoint
            {
                Path = "/customers",
                Method = "POST",
                RequestType = typeof(CreateCustomerRequest),
                ResponseType = typeof(CustomerResponse),
                Handler = CreateCustomerHandler
            },
            // PUT /customers/{id}
            new SAPEndpoint
            {
                Path = "/customers/{id}",
                Method = "PUT",
                RequestType = typeof(UpdateCustomerRequest),
                ResponseType = typeof(CustomerResponse),
                Handler = UpdateCustomerHandler
            },
            // DELETE /customers/{id}
            new SAPEndpoint
            {
                Path = "/customers/{id}",
                Method = "DELETE",
                RequestType = typeof(object),
                ResponseType = typeof(object),
                Handler = DeleteCustomerHandler
            },
            #endregion

            #region Sales Order Endpoints
            // GET /sales-orders/{id}
            new SAPEndpoint
            {
                Path = "/sales-orders/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(SalesOrderResponse),
                Handler = GetSalesOrderHandler
            },
            // GET /sales-orders
            new SAPEndpoint
            {
                Path = "/sales-orders",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(SalesOrderListResponse),
                Handler = ListSalesOrdersHandler
            },
            // POST /sales-orders
            new SAPEndpoint
            {
                Path = "/sales-orders",
                Method = "POST",
                RequestType = typeof(CreateSalesOrderRequest),
                ResponseType = typeof(SalesOrderResponse),
                Handler = CreateSalesOrderHandler
            },
            // PUT /sales-orders/{id}
            new SAPEndpoint
            {
                Path = "/sales-orders/{id}",
                Method = "PUT",
                RequestType = typeof(CreateSalesOrderRequest),
                ResponseType = typeof(SalesOrderResponse),
                Handler = UpdateSalesOrderHandler
            },
            // DELETE /sales-orders/{id}
            new SAPEndpoint
            {
                Path = "/sales-orders/{id}",
                Method = "DELETE",
                RequestType = typeof(object),
                ResponseType = typeof(object),
                Handler = DeleteSalesOrderHandler
            },
            #endregion

            #region Delivery Endpoints
            // GET /deliveries/{id}
            new SAPEndpoint
            {
                Path = "/deliveries/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(DeliveryResponse),
                Handler = GetDeliveryHandler
            },
            // GET /deliveries
            new SAPEndpoint
            {
                Path = "/deliveries",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(DeliveryListResponse),
                Handler = ListDeliveriesHandler
            },
            // POST /deliveries (Create delivery from sales order)
            new SAPEndpoint
            {
                Path = "/deliveries",
                Method = "POST",
                RequestType = typeof(object),
                ResponseType = typeof(DeliveryResponse),
                Handler = CreateDeliveryHandler
            },
            // PUT /deliveries/{id}
            new SAPEndpoint
            {
                Path = "/deliveries/{id}",
                Method = "PUT",
                RequestType = typeof(object),
                ResponseType = typeof(DeliveryResponse),
                Handler = UpdateDeliveryHandler
            },
            // DELETE /deliveries/{id}
            new SAPEndpoint
            {
                Path = "/deliveries/{id}",
                Method = "DELETE",
                RequestType = typeof(object),
                ResponseType = typeof(object),
                Handler = DeleteDeliveryHandler
            },
            #endregion

            #region Invoice Endpoints
            // GET /invoices/{id}
            new SAPEndpoint
            {
                Path = "/invoices/{id}",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(InvoiceResponse),
                Handler = GetInvoiceHandler
            },
            // GET /invoices
            new SAPEndpoint
            {
                Path = "/invoices",
                Method = "GET",
                RequestType = typeof(object),
                ResponseType = typeof(InvoiceListResponse),
                Handler = ListInvoicesHandler
            },
            // POST /invoices (Create invoice from delivery)
            new SAPEndpoint
            {
                Path = "/invoices",
                Method = "POST",
                RequestType = typeof(object),
                ResponseType = typeof(InvoiceResponse),
                Handler = CreateInvoiceHandler
            },
            // PUT /invoices/{id}
            new SAPEndpoint
            {
                Path = "/invoices/{id}",
                Method = "PUT",
                RequestType = typeof(object),
                ResponseType = typeof(InvoiceResponse),
                Handler = UpdateInvoiceHandler
            },
            // DELETE /invoices/{id}
            new SAPEndpoint
            {
                Path = "/invoices/{id}",
                Method = "DELETE",
                RequestType = typeof(object),
                ResponseType = typeof(object),
                Handler = DeleteInvoiceHandler
            },
            #endregion

            #region Business Flow Endpoints
            // POST /sales-orders/{id}/create-delivery
            new SAPEndpoint
            {
                Path = "/sales-orders/{id}/create-delivery",
                Method = "POST",
                RequestType = typeof(object),
                ResponseType = typeof(DeliveryResponse),
                Handler = CreateDeliveryFromSalesOrderHandler
            },
            // POST /deliveries/{id}/create-invoice
            new SAPEndpoint
            {
                Path = "/deliveries/{id}/create-invoice",
                Method = "POST",
                RequestType = typeof(object),
                ResponseType = typeof(InvoiceResponse),
                Handler = CreateInvoiceFromDeliveryHandler
            },
            #endregion
        };
    }

    #region Customer Operations

    /// <summary>
    /// Retrieves a specific customer by its ID.
    /// </summary>
    /// <param name="customerId">Customer ID to retrieve.</param>
    /// <returns>Customer response or error response.</returns>
    public async Task<object> GetCustomerAsync(string customerId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return CreateErrorResponse("SD001", "Customer ID is required", "Customer ID parameter cannot be empty");
            }

            var allCustomers = await GetAllCustomersAsync();
            var customer = allCustomers.FirstOrDefault(c => c.CustomerNumber == customerId);

            if (customer == null)
            {
                return CreateErrorResponse("SD002", "Customer not found", $"Customer with ID {customerId} does not exist");
            }

            return MapCustomerToResponse(customer);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("SD999", "Internal error", $"An error occurred while retrieving customer: {ex.Message}");
        }
    }

    /// <summary>
    /// Lists customers with pagination and filtering support.
    /// </summary>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 50).</param>
    /// <param name="customerGroup">Filter by customer group (optional).</param>
    /// <param name="salesOrganization">Filter by sales organization (optional).</param>
    /// <param name="searchTerm">Search term for customer name or search term field (optional).</param>
    /// <returns>Paginated list of customers.</returns>
    public async Task<object> ListCustomersAsync(int page = 1, int pageSize = 50, string? customerGroup = null, string? salesOrganization = null, string? searchTerm = null)
    {
        try
        {
            // Validate pagination parameters
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 50;

            var allCustomers = await GetAllCustomersAsync();

            // Apply filters
            var filteredCustomers = allCustomers.Where(c => !c.DeletionFlag);

            if (!string.IsNullOrWhiteSpace(customerGroup))
            {
                filteredCustomers = filteredCustomers.Where(c => c.CustomerGroup.Equals(customerGroup, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(salesOrganization))
            {
                filteredCustomers = filteredCustomers.Where(c => c.SalesOrganization.Equals(salesOrganization, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredCustomers = filteredCustomers.Where(c => 
                    c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.SearchTerm.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.CustomerNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = filteredCustomers.Count();
            var customers = filteredCustomers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapCustomerToResponse)
                .ToList();

            return new CustomerListResponse
            {
                Customers = customers,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("SD999", "Internal error", $"An error occurred while listing customers: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="request">Create customer request.</param>
    /// <returns>Created customer response or error response.</returns>
    public async Task<object> CreateCustomerAsync(CreateCustomerRequest request)
    {
        try
        {
            // Validate request
            var validationResult = ValidateCreateCustomerRequest(request);
            if (validationResult != null)
                return validationResult;

            // Generate customer number if not provided
            var customerNumber = request.CustomerNumber ?? await GenerateCustomerNumberAsync();

            // Get all customers and check if customer already exists
            var allCustomers = await GetAllCustomersAsync();
            var existingCustomer = allCustomers.FirstOrDefault(c => c.CustomerNumber == customerNumber);
            
            if (existingCustomer != null)
            {
                return CreateErrorResponse("SD003", "Customer already exists", $"Customer with number {customerNumber} already exists");
            }

            var customer = new Customer
            {
                CustomerNumber = customerNumber,
                Name = request.Name,
                Name2 = request.Name2,
                SearchTerm = request.SearchTerm,
                City = request.City,
                PostalCode = request.PostalCode,
                Country = request.Country,
                Region = request.Region,
                Street = request.Street,
                CustomerGroup = request.CustomerGroup,
                SalesOrganization = request.SalesOrganization,
                DistributionChannel = request.DistributionChannel,
                Division = request.Division,
                Currency = request.Currency,
                PaymentTerms = request.PaymentTerms,
                CreditLimit = request.CreditLimit,
                Telephone = request.Telephone,
                Email = request.Email,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "SYSTEM",
                LastChangedOn = DateTime.UtcNow,
                LastChangedBy = "SYSTEM"
            };

            // Add to collection and save
            var customerList = allCustomers.ToList();
            customerList.Add(customer);
            await SaveAllCustomersAsync(customerList);

            return MapCustomerToResponse(customer);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("SD999", "Internal error", $"An error occurred while creating customer: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="customerId">Customer ID to update.</param>
    /// <param name="request">Update customer request.</param>
    /// <returns>Updated customer response or error response.</returns>
    public async Task<object> UpdateCustomerAsync(string customerId, UpdateCustomerRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return CreateErrorResponse("SD001", "Customer ID is required", "Customer ID parameter cannot be empty");
            }

            // Get all customers and find the specific one
            var allCustomers = await GetAllCustomersAsync();
            var customer = allCustomers.FirstOrDefault(c => c.CustomerNumber == customerId);

            if (customer == null)
            {
                return CreateErrorResponse("SD002", "Customer not found", $"Customer with ID {customerId} does not exist");
            }

            // Check if customer is marked for deletion
            if (customer.DeletionFlag)
            {
                return CreateErrorResponse("SD004", "Customer is marked for deletion", $"Customer {customerId} is marked for deletion and cannot be updated");
            }

            // Update only provided fields
            if (!string.IsNullOrWhiteSpace(request.Name))
                customer.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.Name2))
                customer.Name2 = request.Name2;
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                customer.SearchTerm = request.SearchTerm;
            if (!string.IsNullOrWhiteSpace(request.City))
                customer.City = request.City;
            if (!string.IsNullOrWhiteSpace(request.PostalCode))
                customer.PostalCode = request.PostalCode;
            if (!string.IsNullOrWhiteSpace(request.Country))
                customer.Country = request.Country;
            if (!string.IsNullOrWhiteSpace(request.Region))
                customer.Region = request.Region;
            if (!string.IsNullOrWhiteSpace(request.Street))
                customer.Street = request.Street;
            if (!string.IsNullOrWhiteSpace(request.Currency))
                customer.Currency = request.Currency;
            if (!string.IsNullOrWhiteSpace(request.PaymentTerms))
                customer.PaymentTerms = request.PaymentTerms;
            if (request.CreditLimit.HasValue)
                customer.CreditLimit = request.CreditLimit.Value;
            if (!string.IsNullOrWhiteSpace(request.Telephone))
                customer.Telephone = request.Telephone;
            if (!string.IsNullOrWhiteSpace(request.Email))
                customer.Email = request.Email;

            customer.LastChangedOn = DateTime.UtcNow;
            customer.LastChangedBy = "SYSTEM";

            // Save the updated collection
            await SaveAllCustomersAsync(allCustomers.ToList());

            return MapCustomerToResponse(customer);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("SD999", "Internal error", $"An error occurred while updating customer: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a customer (marks it for deletion).
    /// </summary>
    /// <param name="customerId">Customer ID to delete.</param>
    /// <returns>Success response or error response.</returns>
    public async Task<object> DeleteCustomerAsync(string customerId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return CreateErrorResponse("SD001", "Customer ID is required", "Customer ID parameter cannot be empty");
            }

            // Get all customers and find the specific one
            var allCustomers = await GetAllCustomersAsync();
            var customer = allCustomers.FirstOrDefault(c => c.CustomerNumber == customerId);

            if (customer == null)
            {
                return CreateErrorResponse("SD002", "Customer not found", $"Customer with ID {customerId} does not exist");
            }

            // Mark for deletion instead of actual deletion (SAP pattern)
            customer.DeletionFlag = true;
            customer.LastChangedOn = DateTime.UtcNow;
            customer.LastChangedBy = "SYSTEM";

            // Save the updated collection
            await SaveAllCustomersAsync(allCustomers.ToList());

            return new { Success = true, Message = $"Customer {customerId} marked for deletion" };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("SD999", "Internal error", $"An error occurred while deleting customer: {ex.Message}");
        }
    }

    #endregion

    #region Sales Order Operations

    /// <summary>
    /// Retrieves a specific sales order by its ID.
    /// </summary>
    /// <param name="salesOrderId">Sales order ID to retrieve.</param>
    /// <returns>Sales order response or error response.</returns>
    public async Task<object> GetSalesOrderAsync(string salesOrderId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(salesOrderId))
            {
                return CreateErrorResponse("SD011", "Sales order ID is required", "Sales order ID parameter cannot be empty");
            }

            var allSalesOrders = await GetAllSalesOrdersAsync();
            var salesOrder = allSalesOrders.FirstOrDefault(so => so.SalesOrderNumber == salesOrderId);

            if (salesOrder == null)
            {
                return CreateErrorResponse("SD012", "Sales order not found", $"Sales order with ID {salesOrderId} does not exist");
            }

            return MapSalesOrderToResponse(salesOrder);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("SD999", "Internal error", $"An error occurred while retrieving sales order: {ex.Message}");
        }
    }

    /// <summary>
    /// Lists sales orders with pagination and filtering support.
    /// </summary>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 50).</param>
    /// <param name="customerNumber">Filter by customer number (optional).</param>
    /// <param name="salesOrganization">Filter by sales organization (optional).</param>
    /// <param name="orderStatus">Filter by order status (optional).</param>
    /// <returns>Paginated list of sales orders.</returns>
    public async Task<object> ListSalesOrdersAsync(int page = 1, int pageSize = 50, string? customerNumber = null, string? salesOrganization = null, string? orderStatus = null)
    {
        try
        {
            // Validate pagination parameters
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 50;

            var allSalesOrders = await GetAllSalesOrdersAsync();

            // Apply filters
            var filteredSalesOrders = allSalesOrders.Where(so => !so.DeletionFlag);

            if (!string.IsNullOrWhiteSpace(customerNumber))
            {
                filteredSalesOrders = filteredSalesOrders.Where(so => so.CustomerNumber.Equals(customerNumber, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(salesOrganization))
            {
                filteredSalesOrders = filteredSalesOrders.Where(so => so.SalesOrganization.Equals(salesOrganization, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(orderStatus))
            {
                filteredSalesOrders = filteredSalesOrders.Where(so => so.OrderStatus.Equals(orderStatus, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = filteredSalesOrders.Count();
            var salesOrders = filteredSalesOrders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(MapSalesOrderToResponse)
                .ToList();

            return new SalesOrderListResponse
            {
                SalesOrders = salesOrders,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("SD999", "Internal error", $"An error occurred while listing sales orders: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new sales order.
    /// </summary>
    /// <param name="request">Create sales order request.</param>
    /// <returns>Created sales order response or error response.</returns>
    public async Task<object> CreateSalesOrderAsync(CreateSalesOrderRequest request)
    {
        try
        {
            // Validate request
            var validationResult = ValidateCreateSalesOrderRequest(request);
            if (validationResult != null)
                return validationResult;

            // Generate sales order number if not provided
            var salesOrderNumber = request.SalesOrderNumber ?? await GenerateSalesOrderNumberAsync();

            // Get all sales orders and check if order already exists
            var allSalesOrders = await GetAllSalesOrdersAsync();
            var existingSalesOrder = allSalesOrders.FirstOrDefault(so => so.SalesOrderNumber == salesOrderNumber);
            
            if (existingSalesOrder != null)
            {
                return CreateErrorResponse("SD013", "Sales order already exists", $"Sales order with number {salesOrderNumber} already exists");
            }

            // Verify customer exists
            var allCustomers = await GetAllCustomersAsync();
            var customer = allCustomers.FirstOrDefault(c => c.CustomerNumber == request.CustomerNumber);
            if (customer == null)
            {
                return CreateErrorResponse("SD014", "Customer not found", $"Customer with number {request.CustomerNumber} does not exist");
            }

            var salesOrder = new SalesOrder
            {
                SalesOrderNumber = salesOrderNumber,
                SalesOrderType = request.SalesOrderType,
                CustomerNumber = request.CustomerNumber,
                PurchaseOrderNumber = request.PurchaseOrderNumber,
                PurchaseOrderDate = request.PurchaseOrderDate,
                SalesOrganization = request.SalesOrganization,
                DistributionChannel = request.DistributionChannel,
                Division = request.Division,
                RequestedDeliveryDate = request.RequestedDeliveryDate,
                Currency = request.Currency,
                PaymentTerms = request.PaymentTerms,
                Incoterms = request.Incoterms,
                IncotermsLocation = request.IncotermsLocation,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "SYSTEM",
                LastChangedOn = DateTime.UtcNow,
                LastChangedBy = "SYSTEM"
            };

            // Add items
            foreach (var itemRequest in request.Items)
            {
                var item = new SalesOrderItem
                {
                    SalesOrderNumber = salesOrderNumber,
                    ItemNumber = itemRequest.ItemNumber,
                    MaterialNumber = itemRequest.MaterialNumber,
                    OrderQuantity = itemRequest.OrderQuantity,
                    SalesUnit = itemRequest.SalesUnit,
                    Plant = itemRequest.Plant,
                    RequestedDeliveryDate = itemRequest.RequestedDeliveryDate,
                    NetPrice = 100.00m, // Default price - in real system would be calculated
                    NetValue = itemRequest.OrderQuantity * 100.00m,
                    TaxAmount = itemRequest.OrderQuantity * 100.00m * 0.19m // 19% tax
                };
                salesOrder.Items.Add(item);
            }

            // Calculate order totals
            salesOrder.NetValue = salesOrder.Items.Sum(i => i.NetValue);
            salesOrder.TaxAmount = salesOrder.Items.Sum(i => i.TaxAmount);
            salesOrder.TotalValue = salesOrder.NetValue + salesOrder.TaxAmount;

            // Add to collection and save
            var salesOrderList = allSalesOrders.ToList();
            salesOrderList.Add(salesOrder);
            await SaveAllSalesOrdersAsync(salesOrderList);

            return MapSalesOrderToResponse(salesOrder);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("SD999", "Internal error", $"An error occurred while creating sales order: {ex.Message}");
        }
    }

    #endregion

    #region Business Flow Operations

    /// <summary>
    /// Creates a delivery from a sales order.
    /// </summary>
    /// <param name="salesOrderId">Sales order ID to create delivery from.</param>
    /// <returns>Created delivery response or error response.</returns>
    public async Task<object> CreateDeliveryFromSalesOrderAsync(string salesOrderId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(salesOrderId))
            {
                return CreateErrorResponse("SD011", "Sales order ID is required", "Sales order ID parameter cannot be empty");
            }

            var allSalesOrders = await GetAllSalesOrdersAsync();
            var salesOrder = allSalesOrders.FirstOrDefault(so => so.SalesOrderNumber == salesOrderId);

            if (salesOrder == null)
            {
                return CreateErrorResponse("SD012", "Sales order not found", $"Sales order with ID {salesOrderId} does not exist");
            }

            // Check if sales order is already fully delivered
            if (salesOrder.DeliveryStatus == "C")
            {
                return CreateErrorResponse("SD015", "Sales order already delivered", $"Sales order {salesOrderId} is already completely delivered");
            }

            // Generate delivery number
            var deliveryNumber = await GenerateDeliveryNumberAsync();

            var delivery = new Delivery
            {
                DeliveryNumber = deliveryNumber,
                DeliveryType = "LF", // Standard delivery
                SalesOrderNumber = salesOrderId,
                ShipToParty = salesOrder.CustomerNumber,
                SoldToParty = salesOrder.CustomerNumber,
                ShippingPoint = "SHP1", // Default shipping point
                PlannedGoodsMovementDate = DateTime.UtcNow.AddDays(1),
                DeliveryDate = DateTime.UtcNow.AddDays(1),
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "SYSTEM",
                LastChangedOn = DateTime.UtcNow,
                LastChangedBy = "SYSTEM"
            };

            // Add delivery items from sales order items
            foreach (var soItem in salesOrder.Items)
            {
                var deliveryItem = new DeliveryItem
                {
                    DeliveryNumber = deliveryNumber,
                    ItemNumber = soItem.ItemNumber,
                    MaterialNumber = soItem.MaterialNumber,
                    MaterialDescription = soItem.MaterialDescription,
                    SalesOrderNumber = salesOrderId,
                    SalesOrderItem = soItem.ItemNumber,
                    DeliveryQuantity = soItem.OrderQuantity,
                    SalesUnit = soItem.SalesUnit,
                    Plant = soItem.Plant,
                    StorageLocation = soItem.StorageLocation
                };
                delivery.Items.Add(deliveryItem);
            }

            // Calculate delivery totals
            delivery.TotalWeight = delivery.Items.Sum(i => i.DeliveryQuantity * 1.0m); // Assume 1 unit weight per item
            delivery.WeightUnit = "KG";

            // Save delivery
            var allDeliveries = await GetAllDeliveriesAsync();
            var deliveryList = allDeliveries.ToList();
            deliveryList.Add(delivery);
            await SaveAllDeliveriesAsync(deliveryList);

            // Update sales order delivery status
            salesOrder.DeliveryStatus = "C"; // Completely delivered
            salesOrder.LastChangedOn = DateTime.UtcNow;
            salesOrder.LastChangedBy = "SYSTEM";

            // Update sales order item delivery status
            foreach (var item in salesOrder.Items)
            {
                item.DeliveryStatus = "C"; // Completely delivered
            }

            await SaveAllSalesOrdersAsync(allSalesOrders.ToList());

            return MapDeliveryToResponse(delivery);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("SD999", "Internal error", $"An error occurred while creating delivery from sales order: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates an invoice from a delivery.
    /// </summary>
    /// <param name="deliveryId">Delivery ID to create invoice from.</param>
    /// <returns>Created invoice response or error response.</returns>
    public async Task<object> CreateInvoiceFromDeliveryAsync(string deliveryId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(deliveryId))
            {
                return CreateErrorResponse("SD021", "Delivery ID is required", "Delivery ID parameter cannot be empty");
            }

            var allDeliveries = await GetAllDeliveriesAsync();
            var delivery = allDeliveries.FirstOrDefault(d => d.DeliveryNumber == deliveryId);

            if (delivery == null)
            {
                return CreateErrorResponse("SD022", "Delivery not found", $"Delivery with ID {deliveryId} does not exist");
            }

            // Check if delivery is already fully billed
            if (delivery.BillingStatus == "C")
            {
                return CreateErrorResponse("SD025", "Delivery already billed", $"Delivery {deliveryId} is already completely billed");
            }

            // Generate invoice number
            var invoiceNumber = await GenerateInvoiceNumberAsync();

            var invoice = new Invoice
            {
                InvoiceNumber = invoiceNumber,
                InvoiceType = "F2", // Standard invoice
                SalesOrderNumber = delivery.SalesOrderNumber,
                DeliveryNumber = deliveryId,
                Payer = delivery.ShipToParty,
                SoldToParty = delivery.SoldToParty,
                BillToParty = delivery.ShipToParty,
                SalesOrganization = "1000", // Default sales organization
                DistributionChannel = "10", // Default distribution channel
                Division = "00", // Default division
                Currency = "USD", // Default currency
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "SYSTEM",
                LastChangedOn = DateTime.UtcNow,
                LastChangedBy = "SYSTEM"
            };

            // Add invoice items from delivery items
            foreach (var deliveryItem in delivery.Items)
            {
                var invoiceItem = new InvoiceItem
                {
                    InvoiceNumber = invoiceNumber,
                    ItemNumber = deliveryItem.ItemNumber,
                    MaterialNumber = deliveryItem.MaterialNumber,
                    MaterialDescription = deliveryItem.MaterialDescription,
                    SalesOrderNumber = delivery.SalesOrderNumber,
                    SalesOrderItem = deliveryItem.SalesOrderItem,
                    DeliveryNumber = deliveryId,
                    DeliveryItem = deliveryItem.ItemNumber,
                    BillingQuantity = deliveryItem.DeliveryQuantity,
                    SalesUnit = deliveryItem.SalesUnit,
                    NetPrice = 100.00m, // Default price
                    NetValue = deliveryItem.DeliveryQuantity * 100.00m,
                    TaxAmount = deliveryItem.DeliveryQuantity * 100.00m * 0.19m, // 19% tax
                    TaxCode = "V1",
                    TaxRate = 0.19m,
                    Plant = deliveryItem.Plant
                };
                invoice.Items.Add(invoiceItem);
            }

            // Calculate invoice totals
            invoice.NetValue = invoice.Items.Sum(i => i.NetValue);
            invoice.TaxAmount = invoice.Items.Sum(i => i.TaxAmount);
            invoice.TotalValue = invoice.NetValue + invoice.TaxAmount;

            // Save invoice
            var allInvoices = await GetAllInvoicesAsync();
            var invoiceList = allInvoices.ToList();
            invoiceList.Add(invoice);
            await SaveAllInvoicesAsync(invoiceList);

            // Update delivery billing status
            delivery.BillingStatus = "C"; // Completely billed
            delivery.LastChangedOn = DateTime.UtcNow;
            delivery.LastChangedBy = "SYSTEM";

            await SaveAllDeliveriesAsync(allDeliveries.ToList());

            return MapInvoiceToResponse(invoice);
        }
        catch (Exception ex)
        {
            return CreateErrorResponse("SD999", "Internal error", $"An error occurred while creating invoice from delivery: {ex.Message}");
        }
    }

    #endregion

    #region Endpoint Handlers

    private async Task<object> GetCustomerHandler(object request)
    {
        var customerId = ExtractIdFromRequest(request);
        return await GetCustomerAsync(customerId);
    }

    private async Task<object> ListCustomersHandler(object request)
    {
        var (page, pageSize, customerGroup, salesOrganization, searchTerm) = ExtractCustomerListParametersFromRequest(request);
        return await ListCustomersAsync(page, pageSize, customerGroup, salesOrganization, searchTerm);
    }

    private async Task<object> CreateCustomerHandler(object request)
    {
        if (request is CreateCustomerRequest createRequest)
        {
            return await CreateCustomerAsync(createRequest);
        }
        return CreateErrorResponse("SD005", "Invalid request", "Invalid request format for customer creation");
    }

    private async Task<object> UpdateCustomerHandler(object request)
    {
        var customerId = ExtractIdFromRequest(request);
        if (request is UpdateCustomerRequest updateRequest)
        {
            return await UpdateCustomerAsync(customerId, updateRequest);
        }
        return CreateErrorResponse("SD005", "Invalid request", "Invalid request format for customer update");
    }

    private async Task<object> DeleteCustomerHandler(object request)
    {
        var customerId = ExtractIdFromRequest(request);
        return await DeleteCustomerAsync(customerId);
    }

    private async Task<object> GetSalesOrderHandler(object request)
    {
        var salesOrderId = ExtractIdFromRequest(request);
        return await GetSalesOrderAsync(salesOrderId);
    }

    private async Task<object> ListSalesOrdersHandler(object request)
    {
        var (page, pageSize, customerNumber, salesOrganization, orderStatus) = ExtractSalesOrderListParametersFromRequest(request);
        return await ListSalesOrdersAsync(page, pageSize, customerNumber, salesOrganization, orderStatus);
    }

    private async Task<object> CreateSalesOrderHandler(object request)
    {
        if (request is CreateSalesOrderRequest createRequest)
        {
            return await CreateSalesOrderAsync(createRequest);
        }
        return CreateErrorResponse("SD015", "Invalid request", "Invalid request format for sales order creation");
    }

    private async Task<object> UpdateSalesOrderHandler(object request)
    {
        var salesOrderId = ExtractIdFromRequest(request);
        // For simplicity, using the same request type for update
        if (request is CreateSalesOrderRequest updateRequest)
        {
            return CreateErrorResponse("SD016", "Not implemented", "Sales order update not implemented yet");
        }
        return CreateErrorResponse("SD015", "Invalid request", "Invalid request format for sales order update");
    }

    private async Task<object> DeleteSalesOrderHandler(object request)
    {
        var salesOrderId = ExtractIdFromRequest(request);
        return CreateErrorResponse("SD017", "Not implemented", "Sales order deletion not implemented yet");
    }

    private async Task<object> GetDeliveryHandler(object request)
    {
        var deliveryId = ExtractIdFromRequest(request);
        return CreateErrorResponse("SD026", "Not implemented", "Delivery retrieval not implemented yet");
    }

    private async Task<object> ListDeliveriesHandler(object request)
    {
        return CreateErrorResponse("SD027", "Not implemented", "Delivery listing not implemented yet");
    }

    private async Task<object> CreateDeliveryHandler(object request)
    {
        return CreateErrorResponse("SD028", "Not implemented", "Delivery creation not implemented yet");
    }

    private async Task<object> UpdateDeliveryHandler(object request)
    {
        return CreateErrorResponse("SD029", "Not implemented", "Delivery update not implemented yet");
    }

    private async Task<object> DeleteDeliveryHandler(object request)
    {
        return CreateErrorResponse("SD030", "Not implemented", "Delivery deletion not implemented yet");
    }

    private async Task<object> GetInvoiceHandler(object request)
    {
        return CreateErrorResponse("SD031", "Not implemented", "Invoice retrieval not implemented yet");
    }

    private async Task<object> ListInvoicesHandler(object request)
    {
        return CreateErrorResponse("SD032", "Not implemented", "Invoice listing not implemented yet");
    }

    private async Task<object> CreateInvoiceHandler(object request)
    {
        return CreateErrorResponse("SD033", "Not implemented", "Invoice creation not implemented yet");
    }

    private async Task<object> UpdateInvoiceHandler(object request)
    {
        return CreateErrorResponse("SD034", "Not implemented", "Invoice update not implemented yet");
    }

    private async Task<object> DeleteInvoiceHandler(object request)
    {
        return CreateErrorResponse("SD035", "Not implemented", "Invoice deletion not implemented yet");
    }

    private async Task<object> CreateDeliveryFromSalesOrderHandler(object request)
    {
        var salesOrderId = ExtractIdFromRequest(request);
        return await CreateDeliveryFromSalesOrderAsync(salesOrderId);
    }

    private async Task<object> CreateInvoiceFromDeliveryHandler(object request)
    {
        var deliveryId = ExtractIdFromRequest(request);
        return await CreateInvoiceFromDeliveryAsync(deliveryId);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets all customers from the collection.
    /// </summary>
    private async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        try
        {
            var wrapper = await _mockDataProvider.GetDataAsync<CustomersCollectionWrapper>("default/default/customerscollectionwrapper");
            return wrapper.Customers;
        }
        catch (Exception)
        {
            return new List<Customer>();
        }
    }

    /// <summary>
    /// Saves all customers to the collection.
    /// </summary>
    private async Task SaveAllCustomersAsync(IEnumerable<Customer> customers)
    {
        var wrapper = new CustomersCollectionWrapper { Customers = customers.ToList() };
        await _mockDataProvider.SaveDataAsync(wrapper);
    }

    /// <summary>
    /// Gets all sales orders from the collection.
    /// </summary>
    private async Task<IEnumerable<SalesOrder>> GetAllSalesOrdersAsync()
    {
        try
        {
            var wrapper = await _mockDataProvider.GetDataAsync<SalesOrdersCollectionWrapper>("default/default/salesorderscollectionwrapper");
            return wrapper.SalesOrders;
        }
        catch (Exception)
        {
            return new List<SalesOrder>();
        }
    }

    /// <summary>
    /// Saves all sales orders to the collection.
    /// </summary>
    private async Task SaveAllSalesOrdersAsync(IEnumerable<SalesOrder> salesOrders)
    {
        var wrapper = new SalesOrdersCollectionWrapper { SalesOrders = salesOrders.ToList() };
        await _mockDataProvider.SaveDataAsync(wrapper);
    }

    /// <summary>
    /// Gets all deliveries from the collection.
    /// </summary>
    private async Task<IEnumerable<Delivery>> GetAllDeliveriesAsync()
    {
        try
        {
            var wrapper = await _mockDataProvider.GetDataAsync<DeliveriesCollectionWrapper>("default/default/deliveriescollectionwrapper");
            return wrapper.Deliveries;
        }
        catch (Exception)
        {
            return new List<Delivery>();
        }
    }

    /// <summary>
    /// Saves all deliveries to the collection.
    /// </summary>
    private async Task SaveAllDeliveriesAsync(IEnumerable<Delivery> deliveries)
    {
        var wrapper = new DeliveriesCollectionWrapper { Deliveries = deliveries.ToList() };
        await _mockDataProvider.SaveDataAsync(wrapper);
    }

    /// <summary>
    /// Gets all invoices from the collection.
    /// </summary>
    private async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
    {
        try
        {
            var wrapper = await _mockDataProvider.GetDataAsync<InvoicesCollectionWrapper>("default/default/invoicescollectionwrapper");
            return wrapper.Invoices;
        }
        catch (Exception)
        {
            return new List<Invoice>();
        }
    }

    /// <summary>
    /// Saves all invoices to the collection.
    /// </summary>
    private async Task SaveAllInvoicesAsync(IEnumerable<Invoice> invoices)
    {
        var wrapper = new InvoicesCollectionWrapper { Invoices = invoices.ToList() };
        await _mockDataProvider.SaveDataAsync(wrapper);
    }

    /// <summary>
    /// Wrapper classes to ensure consistent naming for collections.
    /// </summary>
    private class CustomersCollectionWrapper
    {
        public List<Customer> Customers { get; set; } = new();
    }

    private class SalesOrdersCollectionWrapper
    {
        public List<SalesOrder> SalesOrders { get; set; } = new();
    }

    private class DeliveriesCollectionWrapper
    {
        public List<Delivery> Deliveries { get; set; } = new();
    }

    private class InvoicesCollectionWrapper
    {
        public List<Invoice> Invoices { get; set; } = new();
    }

    /// <summary>
    /// Maps a Customer entity to a CustomerResponse.
    /// </summary>
    private static CustomerResponse MapCustomerToResponse(Customer customer)
    {
        return new CustomerResponse
        {
            CustomerNumber = customer.CustomerNumber,
            Name = customer.Name,
            Name2 = customer.Name2,
            SearchTerm = customer.SearchTerm,
            City = customer.City,
            PostalCode = customer.PostalCode,
            Country = customer.Country,
            Region = customer.Region,
            Street = customer.Street,
            CustomerGroup = customer.CustomerGroup,
            SalesOrganization = customer.SalesOrganization,
            DistributionChannel = customer.DistributionChannel,
            Division = customer.Division,
            Currency = customer.Currency,
            PaymentTerms = customer.PaymentTerms,
            CreditLimit = customer.CreditLimit,
            Telephone = customer.Telephone,
            Email = customer.Email,
            CreatedOn = customer.CreatedOn,
            CreatedBy = customer.CreatedBy,
            LastChangedOn = customer.LastChangedOn,
            LastChangedBy = customer.LastChangedBy,
            DeletionFlag = customer.DeletionFlag,
            BlockedFlag = customer.BlockedFlag
        };
    }

    /// <summary>
    /// Maps a SalesOrder entity to a SalesOrderResponse.
    /// </summary>
    private static SalesOrderResponse MapSalesOrderToResponse(SalesOrder salesOrder)
    {
        return new SalesOrderResponse
        {
            SalesOrderNumber = salesOrder.SalesOrderNumber,
            SalesOrderType = salesOrder.SalesOrderType,
            CustomerNumber = salesOrder.CustomerNumber,
            PurchaseOrderNumber = salesOrder.PurchaseOrderNumber,
            PurchaseOrderDate = salesOrder.PurchaseOrderDate,
            SalesOrganization = salesOrder.SalesOrganization,
            DistributionChannel = salesOrder.DistributionChannel,
            Division = salesOrder.Division,
            OrderDate = salesOrder.OrderDate,
            RequestedDeliveryDate = salesOrder.RequestedDeliveryDate,
            Currency = salesOrder.Currency,
            NetValue = salesOrder.NetValue,
            TaxAmount = salesOrder.TaxAmount,
            TotalValue = salesOrder.TotalValue,
            PaymentTerms = salesOrder.PaymentTerms,
            Incoterms = salesOrder.Incoterms,
            IncotermsLocation = salesOrder.IncotermsLocation,
            OrderStatus = salesOrder.OrderStatus,
            DeliveryStatus = salesOrder.DeliveryStatus,
            BillingStatus = salesOrder.BillingStatus,
            Items = salesOrder.Items.Select(MapSalesOrderItemToResponse).ToList(),
            CreatedOn = salesOrder.CreatedOn,
            CreatedBy = salesOrder.CreatedBy,
            LastChangedOn = salesOrder.LastChangedOn,
            LastChangedBy = salesOrder.LastChangedBy,
            DeletionFlag = salesOrder.DeletionFlag
        };
    }

    /// <summary>
    /// Maps a SalesOrderItem entity to a SalesOrderItemResponse.
    /// </summary>
    private static SalesOrderItemResponse MapSalesOrderItemToResponse(SalesOrderItem item)
    {
        return new SalesOrderItemResponse
        {
            ItemNumber = item.ItemNumber,
            MaterialNumber = item.MaterialNumber,
            MaterialDescription = item.MaterialDescription,
            OrderQuantity = item.OrderQuantity,
            SalesUnit = item.SalesUnit,
            NetPrice = item.NetPrice,
            PriceUnit = item.PriceUnit,
            NetValue = item.NetValue,
            TaxAmount = item.TaxAmount,
            Plant = item.Plant,
            StorageLocation = item.StorageLocation,
            RequestedDeliveryDate = item.RequestedDeliveryDate,
            ConfirmedDeliveryDate = item.ConfirmedDeliveryDate,
            ItemStatus = item.ItemStatus,
            DeliveryStatus = item.DeliveryStatus,
            BillingStatus = item.BillingStatus,
            DeletionFlag = item.DeletionFlag
        };
    }

    /// <summary>
    /// Maps a Delivery entity to a DeliveryResponse.
    /// </summary>
    private static DeliveryResponse MapDeliveryToResponse(Delivery delivery)
    {
        return new DeliveryResponse
        {
            DeliveryNumber = delivery.DeliveryNumber,
            DeliveryType = delivery.DeliveryType,
            SalesOrderNumber = delivery.SalesOrderNumber,
            ShipToParty = delivery.ShipToParty,
            SoldToParty = delivery.SoldToParty,
            ShippingPoint = delivery.ShippingPoint,
            PlannedGoodsMovementDate = delivery.PlannedGoodsMovementDate,
            ActualGoodsMovementDate = delivery.ActualGoodsMovementDate,
            DeliveryDate = delivery.DeliveryDate,
            DeliveryStatus = delivery.DeliveryStatus,
            PickingStatus = delivery.PickingStatus,
            PackingStatus = delivery.PackingStatus,
            GoodsIssueStatus = delivery.GoodsIssueStatus,
            BillingStatus = delivery.BillingStatus,
            Route = delivery.Route,
            ShippingCondition = delivery.ShippingCondition,
            TotalWeight = delivery.TotalWeight,
            WeightUnit = delivery.WeightUnit,
            TotalVolume = delivery.TotalVolume,
            VolumeUnit = delivery.VolumeUnit,
            Items = delivery.Items.Select(MapDeliveryItemToResponse).ToList(),
            CreatedOn = delivery.CreatedOn,
            CreatedBy = delivery.CreatedBy,
            LastChangedOn = delivery.LastChangedOn,
            LastChangedBy = delivery.LastChangedBy,
            DeletionFlag = delivery.DeletionFlag
        };
    }

    /// <summary>
    /// Maps a DeliveryItem entity to a DeliveryItemResponse.
    /// </summary>
    private static DeliveryItemResponse MapDeliveryItemToResponse(DeliveryItem item)
    {
        return new DeliveryItemResponse
        {
            ItemNumber = item.ItemNumber,
            MaterialNumber = item.MaterialNumber,
            MaterialDescription = item.MaterialDescription,
            SalesOrderNumber = item.SalesOrderNumber,
            SalesOrderItem = item.SalesOrderItem,
            DeliveryQuantity = item.DeliveryQuantity,
            SalesUnit = item.SalesUnit,
            Plant = item.Plant,
            StorageLocation = item.StorageLocation,
            Batch = item.Batch,
            PickingQuantity = item.PickingQuantity,
            PackedQuantity = item.PackedQuantity,
            IssuedQuantity = item.IssuedQuantity,
            ItemStatus = item.ItemStatus,
            PickingStatus = item.PickingStatus,
            PackingStatus = item.PackingStatus,
            GoodsIssueStatus = item.GoodsIssueStatus,
            DeletionFlag = item.DeletionFlag
        };
    }

    /// <summary>
    /// Maps an Invoice entity to an InvoiceResponse.
    /// </summary>
    private static InvoiceResponse MapInvoiceToResponse(Invoice invoice)
    {
        return new InvoiceResponse
        {
            InvoiceNumber = invoice.InvoiceNumber,
            InvoiceType = invoice.InvoiceType,
            SalesOrderNumber = invoice.SalesOrderNumber,
            DeliveryNumber = invoice.DeliveryNumber,
            Payer = invoice.Payer,
            SoldToParty = invoice.SoldToParty,
            BillToParty = invoice.BillToParty,
            SalesOrganization = invoice.SalesOrganization,
            DistributionChannel = invoice.DistributionChannel,
            Division = invoice.Division,
            InvoiceDate = invoice.InvoiceDate,
            PostingDate = invoice.PostingDate,
            DueDate = invoice.DueDate,
            Currency = invoice.Currency,
            NetValue = invoice.NetValue,
            TaxAmount = invoice.TaxAmount,
            TotalValue = invoice.TotalValue,
            PaymentTerms = invoice.PaymentTerms,
            PaymentMethod = invoice.PaymentMethod,
            AccountingDocumentNumber = invoice.AccountingDocumentNumber,
            FiscalYear = invoice.FiscalYear,
            CompanyCode = invoice.CompanyCode,
            InvoiceStatus = invoice.InvoiceStatus,
            PaymentStatus = invoice.PaymentStatus,
            CancelledFlag = invoice.CancelledFlag,
            CancellationInvoiceNumber = invoice.CancellationInvoiceNumber,
            PrintStatus = invoice.PrintStatus,
            EDIStatus = invoice.EDIStatus,
            Items = invoice.Items.Select(MapInvoiceItemToResponse).ToList(),
            CreatedOn = invoice.CreatedOn,
            CreatedBy = invoice.CreatedBy,
            LastChangedOn = invoice.LastChangedOn,
            LastChangedBy = invoice.LastChangedBy,
            DeletionFlag = invoice.DeletionFlag
        };
    }

    /// <summary>
    /// Maps an InvoiceItem entity to an InvoiceItemResponse.
    /// </summary>
    private static InvoiceItemResponse MapInvoiceItemToResponse(InvoiceItem item)
    {
        return new InvoiceItemResponse
        {
            ItemNumber = item.ItemNumber,
            MaterialNumber = item.MaterialNumber,
            MaterialDescription = item.MaterialDescription,
            SalesOrderNumber = item.SalesOrderNumber,
            SalesOrderItem = item.SalesOrderItem,
            DeliveryNumber = item.DeliveryNumber,
            DeliveryItem = item.DeliveryItem,
            BillingQuantity = item.BillingQuantity,
            SalesUnit = item.SalesUnit,
            NetPrice = item.NetPrice,
            PriceUnit = item.PriceUnit,
            NetValue = item.NetValue,
            TaxAmount = item.TaxAmount,
            TaxCode = item.TaxCode,
            TaxRate = item.TaxRate,
            Plant = item.Plant,
            ProfitCenter = item.ProfitCenter,
            CostCenter = item.CostCenter,
            GLAccount = item.GLAccount,
            ItemStatus = item.ItemStatus,
            DeletionFlag = item.DeletionFlag
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
            MessageClass = "SD",
            MessageNumber = errorCode,
            MessageVariables = new List<string>()
        };
    }

    /// <summary>
    /// Validates the create customer request.
    /// </summary>
    private static SAPErrorResponse? ValidateCreateCustomerRequest(CreateCustomerRequest request)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(request, context, results, true))
        {
            var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            return new SAPErrorResponse
            {
                ErrorCode = "SD005",
                Message = "Validation failed",
                Details = errors,
                Type = "E",
                MessageClass = "SD",
                MessageNumber = "005"
            };
        }

        return null;
    }

    /// <summary>
    /// Validates the create sales order request.
    /// </summary>
    private static SAPErrorResponse? ValidateCreateSalesOrderRequest(CreateSalesOrderRequest request)
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(request, context, results, true))
        {
            var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            return new SAPErrorResponse
            {
                ErrorCode = "SD015",
                Message = "Validation failed",
                Details = errors,
                Type = "E",
                MessageClass = "SD",
                MessageNumber = "015"
            };
        }

        if (request.Items == null || !request.Items.Any())
        {
            return new SAPErrorResponse
            {
                ErrorCode = "SD016",
                Message = "Sales order items are required",
                Details = "Sales order must have at least one item",
                Type = "E",
                MessageClass = "SD",
                MessageNumber = "016"
            };
        }

        return null;
    }

    /// <summary>
    /// Generates a new customer number.
    /// </summary>
    private Task<string> GenerateCustomerNumberAsync()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(100, 999);
        return Task.FromResult($"CUST{timestamp}{random}");
    }

    /// <summary>
    /// Generates a new sales order number.
    /// </summary>
    private Task<string> GenerateSalesOrderNumberAsync()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(100, 999);
        return Task.FromResult($"SO{timestamp}{random}");
    }

    /// <summary>
    /// Generates a new delivery number.
    /// </summary>
    private Task<string> GenerateDeliveryNumberAsync()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(100, 999);
        return Task.FromResult($"DEL{timestamp}{random}");
    }

    /// <summary>
    /// Generates a new invoice number.
    /// </summary>
    private Task<string> GenerateInvoiceNumberAsync()
    {
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(100, 999);
        return Task.FromResult($"INV{timestamp}{random}");
    }

    /// <summary>
    /// Extracts ID from request context.
    /// This is a placeholder - in a real implementation, this would extract from route parameters.
    /// </summary>
    private static string ExtractIdFromRequest(object request)
    {
        // In a real implementation, this would extract from the HTTP context or route parameters
        // For now, return a placeholder
        return "DEFAULT_ID";
    }

    /// <summary>
    /// Extracts customer list parameters from request context.
    /// This is a placeholder - in a real implementation, this would extract from query parameters.
    /// </summary>
    private static (int page, int pageSize, string? customerGroup, string? salesOrganization, string? searchTerm) ExtractCustomerListParametersFromRequest(object request)
    {
        // In a real implementation, this would extract from query parameters
        // For now, return default values
        return (1, 50, null, null, null);
    }

    /// <summary>
    /// Extracts sales order list parameters from request context.
    /// This is a placeholder - in a real implementation, this would extract from query parameters.
    /// </summary>
    private static (int page, int pageSize, string? customerNumber, string? salesOrganization, string? orderStatus) ExtractSalesOrderListParametersFromRequest(object request)
    {
        // In a real implementation, this would extract from query parameters
        // For now, return default values
        return (1, 50, null, null, null);
    }

    #endregion
}