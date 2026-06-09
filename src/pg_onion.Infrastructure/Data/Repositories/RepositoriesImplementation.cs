// Concrete Repository Implementations with Entity Framework Core
namespace pg_onion.Infrastructure.Data.Repositories
{
    using Domain.Entities;
    using Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class FinancialAccountRepository : IFinancialAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public FinancialAccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FinancialAccount>> GetByUserIdAsync(int userId)
        {
            return await _context.FinancialAccounts
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<FinancialAccount> GetByIdAsync(int id)
        {
            return await _context.FinancialAccounts
                .Include(a => a.Transactions)
                .Include(a => a.Budgets)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(FinancialAccount account)
        {
            await _context.FinancialAccounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FinancialAccount account)
        {
            _context.FinancialAccounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var account = await GetByIdAsync(id);
            if (account != null)
            {
                _context.FinancialAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetByAccountIdAsync(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .Include(t => t.ItemOfExpenses)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByItemOfExpensesIdAsync(int itemId)
        {
            return await _context.Transactions
                .Where(t => t.ItemOfExpensesId == itemId)
                .Include(t => t.ItemOfExpenses)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByPeriodAsync(DateTime start, DateTime end)
        {
            return await _context.Transactions
                .Where(t => t.TransactionDate >= start && t.TransactionDate <= end)
                .Include(t => t.ItemOfExpenses)
                .ToListAsync();
        }

        public async Task<Transaction> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.ItemOfExpenses)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await GetByIdAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class ItemOfExpensesRepository : IItemOfExpensesRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemOfExpensesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemOfExpenses>> GetAllAsync()
        {
            return await _context.ItemOfExpenses.ToListAsync();
        }

        public async Task<List<ItemOfExpenses>> GetByTypeAsync(TransactionType type)
        {
            return await _context.ItemOfExpenses
                .Where(i => i.Type == type)
                .ToListAsync();
        }

        public async Task<ItemOfExpenses> GetByIdAsync(int id)
        {
            return await _context.ItemOfExpenses.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(ItemOfExpenses item)
        {
            await _context.ItemOfExpenses.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ItemOfExpenses item)
        {
            _context.ItemOfExpenses.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await GetByIdAsync(id);
            if (item != null)
            {
                _context.ItemOfExpenses.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class BudgetRepository : IBudgetRepository
    {
        private readonly ApplicationDbContext _context;

        public BudgetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Budget>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Budgets
                .Where(b => b.ItemOfExpensesId == categoryId)
                .ToListAsync();
        }

        public async Task<Budget> GetActiveBudgetAsync(int categoryId, DateTime date)
        {
            return await _context.Budgets
                .Where(b => b.ItemOfExpensesId == categoryId &&
                           b.StartDate <= date &&
                           b.EndDate >= date)
                .FirstOrDefaultAsync();
        }

        public async Task<Budget> GetByIdAsync(int id)
        {
            return await _context.Budgets
                .Include(b => b.ItemOfExpenses)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Budget budget)
        {
            await _context.Budgets.AddAsync(budget);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Budget budget)
        {
            _context.Budgets.Update(budget);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var budget = await GetByIdAsync(id);
            if (budget != null)
            {
                _context.Budgets.Remove(budget);
                await _context.SaveChangesAsync();
            }
        }
    }
}
