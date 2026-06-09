// Updated Application DTOs with enhanced specifications
namespace pg_onion.Application.DTOs
{
    // User DTOs
    public class UserRegistrationDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    // Financial Account DTOs
    public class CreateFinancialAccountDto
    {
        public string AccountName { get; set; }
        public decimal InitialBalance { get; set; }
        public string Currency { get; set; }
    }

    public class FinancialAccountDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Transaction DTOs
    public class CreateTransactionDto
    {
        public int AccountId { get; set; }
        public int ItemOfExpensesId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
    }

    public class TransactionDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public ItemOfExpensesDto ItemOfExpenses { get; set; }
    }

    // ItemOfExpenses DTOs
    public class ItemOfExpensesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int CategoryId { get; set; }
    }

    // Budget DTOs
    public class CreateBudgetDto
    {
        public int FinancialAccountId { get; set; }
        public int ItemOfExpensesId { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class BudgetDto
    {
        public int Id { get; set; }
        public int ItemOfExpensesId { get; set; }
        public decimal Amount { get; set; }
        public decimal SpentAmount { get; set; }
        public bool IsExceeded => SpentAmount > Amount;
        public decimal RemainingAmount => Amount - SpentAmount;
        public double PercentageUsed => Amount > 0 ? (double)SpentAmount / (double)Amount * 100 : 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    // Report DTOs - Grouped by Category
    public class TransactionByCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }

    public class ReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AccountId { get; set; }
        public List<TransactionByCategoryDto> TransactionsByCategory { get; set; } = new();
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetBalance { get; set; }
        public List<BudgetDto> Budgets { get; set; } = new();
    }
}
