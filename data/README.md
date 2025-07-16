# SAP Mock Data Structure

This directory contains sample mock data for the SAP Mock Service, organized by system and module to support comprehensive testing and development scenarios.

## Directory Structure

```
data/
├── common/                          # Shared mock data accessible to all profiles
│   ├── ERP01/                      # SAP ERP System (Original)
│   │   ├── MM/                     # Materials Management
│   │   │   └── materials.json      # Material master data (5 samples)
│   │   ├── SD/                     # Sales & Distribution
│   │   │   ├── customers.json      # Customer master data (10 samples)
│   │   │   ├── sales-orders.json   # Sales orders with line items (5 samples)
│   │   │   ├── deliveries.json     # Delivery documents
│   │   │   └── invoices.json       # Invoice documents
│   │   └── FI/                     # Financial Accounting
│   │       └── general-ledger.json # General ledger accounts (5 samples)
│   ├── ERP-DEV/                    # SAP ERP Development Environment
│   │   ├── MM/                     # Materials Management
│   │   │   └── materials.json      # Material master data
│   │   ├── SD/                     # Sales & Distribution
│   │   │   ├── customers.json      # Customer master data
│   │   │   └── sales-orders.json   # Sales orders with line items
│   │   └── FI/                     # Financial Accounting
│   │       └── general-ledger.json # General ledger accounts
│   ├── ERP-PROD/                   # SAP ERP Production Environment
│   │   ├── MM/                     # Materials Management
│   │   │   └── materials.json      # Material master data
│   │   ├── SD/                     # Sales & Distribution
│   │   │   ├── customers.json      # Customer master data
│   │   │   └── sales-orders.json   # Sales orders with line items
│   │   └── FI/                     # Financial Accounting
│   │       └── general-ledger.json # General ledger accounts
│   └── S4HANA/                     # SAP S/4HANA Environment
│       ├── MM/                     # Materials Management
│       │   └── materials.json      # Material master data
│       ├── SD/                     # Sales & Distribution
│       │   ├── customers.json      # Customer master data
│       │   └── sales-orders.json   # Sales orders with line items
│       └── FI/                     # Financial Accounting
│           └── general-ledger.json # General ledger accounts
└── extensions/                     # Developer-specific data extensions
    └── [profile]/                  # Profile-specific overrides
        └── [system]/               # System-specific overrides
            └── [module]/           # Module-specific overrides
```

## Sample Data Overview

### Materials Management (MM)
- **5 sample materials** covering different material types:
  - MATERIAL-001: Common Test Material 1 (FERT - Finished Product)
  - MATERIAL-002: Common Test Material 2 (FERT - Finished Product)
  - MATERIAL-003: Steel Rod Premium Grade (ROH - Raw Material)
  - MATERIAL-004: Electronic Component Assembly (HALB - Semi-finished Product)
  - MATERIAL-005: Packaging Material - Cardboard Box (VERP - Packaging Material)

### Sales & Distribution (SD)
- **10 sample customers** representing various business scenarios:
  - CUST001-CUST010: Mix of domestic and international customers
  - Different customer groups (Z001-Z005)
  - Various payment terms (NET15, NET30, NET45, NET60)
  - Different sales organizations and distribution channels

- **5 sample sales orders** with line items:
  - SO001-SO005: Orders from different customers
  - Multiple line items per order
  - Various order statuses and currencies
  - Different delivery and billing scenarios

### Financial Accounting (FI)
- **5 sample general ledger accounts**:
  - 100000: Cash and Cash Equivalents (Assets)
  - 110000: Accounts Receivable (Assets)
  - 200000: Accounts Payable (Liabilities)
  - 300000: Common Stock (Equity)
  - 400000: Sales Revenue (Revenue)

## System Configurations

### ERP01 (Original SAP ERP)
- **Server**: sap-erp-dev.company.com:8000
- **Client**: 100
- **Modules**: MM, SD, FI
- **Endpoints**: REST-based API endpoints

### ERP-DEV (Development Environment)
- **Server**: sap-erp-dev.company.com:8000
- **Client**: 100
- **Modules**: MM, SD, FI
- **Endpoints**: REST-based API endpoints

### ERP-PROD (Production Environment)
- **Server**: sap-erp-prod.company.com:8000
- **Client**: 200
- **Modules**: MM, SD, FI
- **Endpoints**: REST-based API endpoints

### S4HANA (SAP S/4HANA Environment)
- **Server**: sap-s4hana.company.com:443 (HTTPS)
- **Client**: 100
- **Modules**: MM, SD, FI
- **Endpoints**: OData-based API endpoints

## Usage Guidelines

### Data Loading Priority
1. **Extensions First**: Developer-specific data in `extensions/[profile]/[system]/[module]/`
2. **Common Fallback**: Shared data in `common/[system]/[module]/`

### Adding Custom Data
1. Create your profile directory under `extensions/`
2. Mirror the system/module structure
3. Add your custom JSON files
4. The mock service will automatically use your data when available

### JSON File Format
All JSON files follow the SAP data structure conventions:
- Consistent field naming (camelCase)
- Proper data types (strings, numbers, booleans, dates)
- ISO 8601 date formats
- Nested objects for complex data structures

### Sample Data Characteristics
- **Realistic**: Data reflects typical SAP business scenarios
- **Diverse**: Covers different business cases and edge scenarios
- **Consistent**: Cross-references between modules (e.g., materials in sales orders)
- **Scalable**: Easy to extend with additional records

## Module-Specific Notes

### MM (Materials Management)
- Material numbers follow SAP conventions (e.g., MATERIAL-001)
- Different material types: FERT, ROH, HALB, VERP
- Units of measure: EA, M, PC, ST, KG
- Weight and dimension data included

### SD (Sales & Distribution)
- Customer numbers follow SAP conventions (e.g., CUST001)
- Sales orders reference existing customers and materials
- Multiple line items per order supported
- Various order statuses and currencies

### FI (Financial Accounting)
- Account numbers follow SAP chart of accounts structure
- Balance sheet and P&L accounts included
- Multiple company codes supported
- Transaction history with timestamps

## Configuration Files

System and module configurations are stored in the `config/` directory:
- `system-[SYSTEM].json`: System-level configuration
- `module-[SYSTEM]-[MODULE].json`: Module-specific configuration

These files define endpoints, handlers, and system-specific parameters.

## Development Tips

1. **Use Extensions**: Keep your custom data in extensions to avoid conflicts
2. **Follow Naming**: Use SAP-like naming conventions for consistency
3. **Maintain Relationships**: Ensure data relationships are maintained across modules
4. **Test Thoroughly**: Validate JSON syntax and data integrity
5. **Document Changes**: Update this README when adding new data structures

## Data Validation

All JSON files are validated for:
- Valid JSON syntax
- Required fields presence
- Data type consistency
- Cross-reference integrity

Use the SAP Mock Service's built-in validation endpoints to verify data integrity.

---

*Last Updated: 2024-02-16*
*Version: 1.0*