# Personal Financial Management System - Onion Architecture

## 📋 Project Overview

This project implements a Personal Financial Management System using **Onion (Hexagonal) Architecture** in .NET. The system allows users to manage multiple financial accounts, track transactions, set budgets, and generate detailed financial reports.

## 🧅 Architecture Layers

### 1. **Domain Layer** (`pg_onion.Domain`)
Core business logic and domain entities - completely independent of any frameworks.

**Key Components:**
- **Entities:**
  - `User` - User accounts
  - `FinancialAccount` - User's bank accounts (multiple per user)
  - `Transaction` - Financial transactions
  - `ItemOfExpenses` - Expense categories with types (Income, Expense, Transfer)
  - `Budget` - Budget limits with period support

- **Interfaces (Abstractions):**
  - `IUserRepository`
  - `IFinancialAccountRepository`
  - `ITransactionRepository`
  - `IItemOfExpensesRepository`
  - `IBudgetRepository`

### 2. **Application Layer** (`pg_onion.Application`)
Business logic implementation and service interfaces - orchestrates domain objects.

**Key Components:**
- **Services:**
  - `UserService` - User registration and authentication
  - `FinancialAccountService` - Account management with pagination
  - `TransactionService` - Transaction management with budget validation
  - `BudgetService` - Budget management and validation
  - `ItemOfExpensesService` - Expense items management
  - `ReportService` - Generate financial reports grouped by categories
  - `AuthService` - JWT token generation and validation

- **DTOs (Data Transfer Objects):**
  - `UserRegistrationDto`, `UserDto`, `AuthResponseDto`
  - `CreateFinancialAccountDto`, `FinancialAccountDto`
  - `CreateTransactionDto`, `TransactionDto`
  - `CreateBudgetDto`, `BudgetDto`
  - `ReportDto`, `TransactionByCategoryDto`
  - `PaginatedResult<T>` - Pagination support (max 20 items per page)

- **Service Interfaces:**
  - `IUserService`
  - `IFinancialAccountService`
  - `ITransactionService`
  - `IBudgetService`
  - `IItemOfExpensesService`
  - `IReportService`
  - `IAuthService`

### 3. **Infrastructure Layer** (`pg_onion.Infrastructure`)
External concerns - database access, external services, persistence.

**Key Components:**
- **Repositories (Template):**
  - `UserRepository`
  - `FinancialAccountRepository`
  - `TransactionRepository`
  - `ItemOfExpensesRepository`
  - `BudgetRepository`

- **To Be Implemented:**
  - Entity Framework Core `DbContext`
  - Database migrations
  - External service clients

### 4. **Presentation Layer** (`pg_onion.Presentation`)
API endpoints and HTTP handling - user-facing layer.

**Key Components:**
- **Controllers:**
  - `AuthController` - User registration and login
  - `AccountsController` - Financial account management
  - `TransactionsController` - Transaction management with period filtering
  - `BudgetsController` - Budget management
  - `ItemsController` - Expense items management
  - `ReportsController` - Financial reports

## 🔐 Authentication & Authorization

- JWT Token-based authentication
- Bearer token in Authorization header: `Authorization: Bearer {token}`
- Tokens expire in 24 hours
- Protected endpoints require valid token

## 💰 Key Features

### 1. **Multi-Account Support**
- One user can have multiple financial accounts
- Each account has its own balance and transactions
- Accounts support different currencies

### 2. **Transaction Management**
- Create, read, and delete transactions
- Automatic budget validation on transaction creation
- Transactions include item of expenses reference
- Period-based filtering with date range (inclusive)
- Paginated results (max 20 per page)

### 3. **Budget Management**
- Multiple budgets per expense item
- Period-based budgets (different periods = different budgets)
- Budget exceeded detection
- Percentage-based usage tracking

### 4. **Financial Reports**
- JSON format reports
- Grouped by expense categories
- Includes:
  - Transaction breakdown by category
  - Total income and expenses
  - Net balance calculation
  - Active budgets with spent amounts

### 5. **Pagination**
- Applied to: Accounts, Transactions (by period)
- Maximum page size: 20 items
- Returns: Items, PageNumber, PageSize, TotalCount, TotalPages, HasPreviousPage, HasNextPage

## 📡 API Endpoints

### Authentication
```
POST   /api/auth/register          - Register new user
POST   /api/auth/login              - Login and get JWT token
```

### Financial Accounts
```
GET    /api/accounts                - Get user's accounts (paginated)
GET    /api/accounts/{id}           - Get specific account
POST   /api/accounts                - Create new account
```

### Transactions
```
GET    /api/accounts/{accountId}/transactions              - Get account transactions (paginated)
GET    /api/accounts/{accountId}/transactions/period       - Get transactions by period (paginated, inclusive dates)
POST   /api/accounts/{accountId}/transactions              - Add transaction (with budget validation)
DELETE /api/accounts/{accountId}/transactions/{id}         - Delete transaction
```

### Budgets
```
POST   /api/budgets                 - Create budget
GET    /api/budgets/item/{itemId}   - Get all budgets for item
GET    /api/budgets/item/{itemId}/check - Check if budget exceeded
```

### Items
```
GET    /api/items                   - Get all items
GET    /api/items/type/{type}       - Get items by type (0=Income, 1=Expense, 2=Transfer)
POST   /api/items                   - Create item
```

### Reports
```
GET    /api/accounts/{accountId}/reports - Generate report (grouped by categories)
```

## 🔄 Data Flow

```
1. Request → Presentation Layer (Controller)
2. Controller → Application Layer (Service)
3. Service → Domain Layer (Business Logic)
4. Domain → Infrastructure Layer (Repository)
5. Repository → Database
6. Response flows back through layers
```

## 📦 Dependencies

- ASP.NET Core 6.0+
- Entity Framework Core (to be implemented)
- JWT Bearer Authentication
- System.IdentityModel.Tokens.Jwt (for token generation)

## 🚀 Getting Started

### Prerequisites
- .NET 6.0+
- SQL Server / PostgreSQL
- Visual Studio or VS Code

### Setup

1. Clone the repository
2. Install NuGet packages
3. Configure database connection in `appsettings.json`
4. Run Entity Framework migrations
5. Start the API

```bash
dotnet restore
dotnet ef database update
dotnet run
```

## 📝 Business Rules

1. **User Creation:** Email must be unique
2. **Budget Validation:** Transactions are rejected if they exceed budget
3. **Budget Periods:** Multiple budgets can exist for same item if periods don't overlap
4. **Transaction Dates:** Start and end dates in period queries are inclusive
5. **Pagination:** Maximum page size is 20; defaults to page 1 with size 20
6. **Account Balance:** Updated automatically when transactions are added/deleted

## 🔧 Future Implementation

- [ ] Real JWT token generation with claims
- [ ] Entity Framework Core repositories
- [ ] Database migrations
- [ ] Password hashing (BCrypt/Argon2)
- [ ] Email verification
- [ ] Multi-factor authentication
- [ ] Advanced filtering and search
- [ ] Export to CSV/PDF
- [ ] Recurring transactions
- [ ] Goal setting
- [ ] Notification system

## 📚 Architecture Benefits

✅ **Testability** - Each layer can be tested independently
✅ **Maintainability** - Clear separation of concerns
✅ **Flexibility** - Easy to swap implementations
✅ **Scalability** - Can add new features without affecting existing code
✅ **Independence** - Domain layer has no external dependencies

## 📄 License

This project is part of a portfolio demonstration.
