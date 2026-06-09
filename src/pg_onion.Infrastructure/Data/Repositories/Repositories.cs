// Infrastructure Repository Implementations
namespace pg_onion.Infrastructure.Data.Repositories
{
    using Domain.Entities;
    using Domain.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    // TODO: Implement with DbContext - this is a template
    public class UserRepository : IUserRepository
    {
        // private readonly ApplicationDbContext _context;

        // public UserRepository(ApplicationDbContext context)
        // {
        //     _context = context;
        // }

        public Task<User> GetByEmailAsync(string email)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task AddAsync(User user)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }
    }

    public class FinancialAccountRepository : IFinancialAccountRepository
    {
        public Task<List<FinancialAccount>> GetByUserIdAsync(int userId)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task<FinancialAccount> GetByIdAsync(int id)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task AddAsync(FinancialAccount account)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task UpdateAsync(FinancialAccount account)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        public Task<List<Transaction>> GetByAccountIdAsync(int accountId)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task<List<Transaction>> GetByItemOfExpensesIdAsync(int itemId)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task<List<Transaction>> GetByPeriodAsync(DateTime start, DateTime end)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task<Transaction> GetByIdAsync(int id)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task AddAsync(Transaction transaction)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task UpdateAsync(Transaction transaction)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }
    }

    public class ItemOfExpensesRepository : IItemOfExpensesRepository
    {
        public Task<List<ItemOfExpenses>> GetAllAsync()
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task<List<ItemOfExpenses>> GetByTypeAsync(TransactionType type)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task<ItemOfExpenses> GetByIdAsync(int id)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task AddAsync(ItemOfExpenses item)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task UpdateAsync(ItemOfExpenses item)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }
    }

    public class BudgetRepository : IBudgetRepository
    {
        public Task<List<Budget>> GetByCategoryIdAsync(int categoryId)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task<Budget> GetActiveBudgetAsync(int categoryId, DateTime date)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task<Budget> GetByIdAsync(int id)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task AddAsync(Budget budget)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task UpdateAsync(Budget budget)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException("Implement with Entity Framework");
        }
    }
}
