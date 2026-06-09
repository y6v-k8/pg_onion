// ItemOfExpenses and Report Services Implementation
namespace pg_onion.Application.Services
{
    using Interfaces;
    using DTOs;
    using Domain.Entities;
    using Domain.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ItemOfExpensesService : IItemOfExpensesService
    {
        private readonly IItemOfExpensesRepository _itemRepository;

        public ItemOfExpensesService(IItemOfExpensesRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<List<ItemOfExpensesDto>> GetAllAsync()
        {
            var items = await _itemRepository.GetAllAsync();
            return items.Select(MapToDto).ToList();
        }

        public async Task<List<ItemOfExpensesDto>> GetByTypeAsync(int type)
        {
            var transactionType = (TransactionType)type;
            var items = await _itemRepository.GetByTypeAsync(transactionType);
            return items.Select(MapToDto).ToList();
        }

        public async Task<ItemOfExpensesDto> CreateAsync(ItemOfExpensesDto dto)
        {
            var item = new ItemOfExpenses
            {
                Name = dto.Name,
                Type = (TransactionType)dto.Type,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _itemRepository.AddAsync(item);

            return new ItemOfExpensesDto
            {
                Id = item.Id,
                Name = item.Name,
                Type = (int)item.Type,
                CategoryId = item.CategoryId
            };
        }

        private ItemOfExpensesDto MapToDto(ItemOfExpenses item)
        {
            return new ItemOfExpensesDto
            {
                Id = item.Id,
                Name = item.Name,
                Type = (int)item.Type,
                CategoryId = item.CategoryId
            };
        }
    }

    public class ReportService : IReportService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IItemOfExpensesRepository _itemRepository;

        public ReportService(
            ITransactionRepository transactionRepository,
            IBudgetRepository budgetRepository,
            IItemOfExpensesRepository itemRepository)
        {
            _transactionRepository = transactionRepository;
            _budgetRepository = budgetRepository;
            _itemRepository = itemRepository;
        }

        public async Task<ReportDto> GenerateAsync(DateTime startDate, DateTime endDate, int accountId)
        {
            // Get all transactions in the period for the account
            var allTransactions = await _transactionRepository.GetByPeriodAsync(startDate, endDate);
            var accountTransactions = allTransactions
                .Where(t => t.AccountId == accountId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .ToList();

            // Group transactions by category/ItemOfExpenses
            var groupedByCategory = accountTransactions
                .GroupBy(t => new { t.ItemOfExpensesId, t.ItemOfExpenses.Name })
                .Select(g => new TransactionByCategoryDto
                {
                    CategoryId = g.Key.ItemOfExpensesId,
                    CategoryName = g.Key.Name,
                    Transactions = g.Select(t => new TransactionDto
                    {
                        Id = t.Id,
                        AccountId = t.AccountId,
                        Amount = t.Amount,
                        TransactionDate = t.TransactionDate,
                        Description = t.Description
                    }).ToList(),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .ToList();

            // Calculate totals
            var totalIncome = accountTransactions
                .Where(t => t.ItemOfExpenses.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var totalExpenses = accountTransactions
                .Where(t => t.ItemOfExpenses.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            var netBalance = totalIncome - totalExpenses;

            // Get budgets for the period
            var allBudgets = await _budgetRepository.GetByCategoryIdAsync(0); // Get all budgets - this might need optimization
            var activeBudgets = allBudgets
                .Where(b => !(b.EndDate < startDate || b.StartDate > endDate))
                .Select(b => new BudgetDto
                {
                    Id = b.Id,
                    ItemOfExpensesId = b.ItemOfExpensesId,
                    Amount = b.Amount,
                    SpentAmount = accountTransactions
                        .Where(t => t.ItemOfExpensesId == b.ItemOfExpensesId &&
                                   t.TransactionDate >= b.StartDate &&
                                   t.TransactionDate <= b.EndDate)
                        .Sum(t => t.Amount),
                    StartDate = b.StartDate,
                    EndDate = b.EndDate
                })
                .ToList();

            return new ReportDto
            {
                StartDate = startDate,
                EndDate = endDate,
                AccountId = accountId,
                TransactionsByCategory = groupedByCategory,
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                NetBalance = netBalance,
                Budgets = activeBudgets
            };
        }
    }
}
