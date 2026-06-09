// Domain Repository Interfaces
namespace pg_onion.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }

    public interface IFinancialAccountRepository
    {
        Task<List<FinancialAccount>> GetByUserIdAsync(int userId);
        Task<FinancialAccount> GetByIdAsync(int id);
        Task AddAsync(FinancialAccount account);
        Task UpdateAsync(FinancialAccount account);
        Task DeleteAsync(int id);
    }

    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetByAccountIdAsync(int accountId);
        Task<List<Transaction>> GetByItemOfExpensesIdAsync(int itemId);
        Task<List<Transaction>> GetByPeriodAsync(DateTime start, DateTime end);
        Task<Transaction> GetByIdAsync(int id);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
    }

    public interface IItemOfExpensesRepository
    {
        Task<List<ItemOfExpenses>> GetAllAsync();
        Task<List<ItemOfExpenses>> GetByTypeAsync(TransactionType type);
        Task<ItemOfExpenses> GetByIdAsync(int id);
        Task AddAsync(ItemOfExpenses item);
        Task UpdateAsync(ItemOfExpenses item);
        Task DeleteAsync(int id);
    }

    public interface IBudgetRepository
    {
        Task<List<Budget>> GetByCategoryIdAsync(int categoryId);
        Task<Budget> GetActiveBudgetAsync(int categoryId, DateTime date);
        Task<Budget> GetByIdAsync(int id);
        Task AddAsync(Budget budget);
        Task UpdateAsync(Budget budget);
        Task DeleteAsync(int id);
    }
}
