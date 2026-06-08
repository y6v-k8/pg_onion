namespace FinancialTracker.Application;

/// <summary>
/// Интерфейс сервиса транзакций.
/// </summary>
public interface ITransactionService
{
    Task AddTransactionAsync(Transaction transaction);
    Task DeleteTransactionAsync(int transactionId);
    Task<FinancialAccount> GetAccountAsync(int accountId);
    Task<List<Transaction>> GetByItemOfExpensesIdAsync(int itemId);
    Task<List<Transaction>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
}
