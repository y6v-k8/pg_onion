namespace FinancialTracker.Application;

/// <summary>
/// Интерфейс репозитория бюджетов.
/// </summary>
public interface IBudgetRepository : IRepository<Budget>
{
    Task<List<Budget>> GetByCategoryIdAsync(int categoryId);
    Task<Budget?> GetActiveBudgetAsync(int categoryId, DateTime date);
}
