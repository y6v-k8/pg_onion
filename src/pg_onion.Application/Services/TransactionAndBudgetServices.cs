// Transaction and Budget Services Implementation
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

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IFinancialAccountRepository _accountRepository;
        private readonly IBudgetRepository _budgetRepository;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IFinancialAccountRepository accountRepository,
            IBudgetRepository budgetRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _budgetRepository = budgetRepository;
        }

        public async Task<TransactionDto> AddTransactionAsync(CreateTransactionDto dto)
        {
            var account = await _accountRepository.GetByIdAsync(dto.AccountId);
            if (account == null)
                throw new KeyNotFoundException($"Account with ID {dto.AccountId} not found.");

            // Check budget limit if expense
            var budget = await _budgetRepository.GetActiveBudgetAsync(dto.ItemOfExpensesId, dto.TransactionDate);
            if (budget != null && budget.SpentAmount + dto.Amount > budget.Amount)
                throw new InvalidOperationException($"Budget exceeded for this item. Available: {budget.Amount - budget.SpentAmount}");

            var transaction = new Transaction
            {
                AccountId = dto.AccountId,
                ItemOfExpensesId = dto.ItemOfExpensesId,
                Amount = dto.Amount,
                TransactionDate = dto.TransactionDate,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _transactionRepository.AddAsync(transaction);

            // Update account balance
            account.Balance -= dto.Amount;
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account);

            return new TransactionDto
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Description = transaction.Description
            };
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
                throw new KeyNotFoundException($"Transaction with ID {id} not found.");

            // Restore account balance
            var account = await _accountRepository.GetByIdAsync(transaction.AccountId);
            account.Balance += transaction.Amount;
            account.UpdatedAt = DateTime.UtcNow;
            await _accountRepository.UpdateAsync(account);

            await _transactionRepository.DeleteAsync(id);
        }

        public async Task<PaginatedResult<TransactionDto>> GetByAccountIdAsync(int accountId, int pageNumber = 1, int pageSize = 20)
        {
            var paginationParams = new PaginationParams { PageNumber = pageNumber, PageSize = pageSize };
            paginationParams.Validate();

            var transactions = await _transactionRepository.GetByAccountIdAsync(accountId);
            var totalCount = transactions.Count;

            var paginatedTransactions = transactions
                .OrderByDescending(t => t.TransactionDate)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            var dtos = paginatedTransactions.Select(MapToDto).ToList();
            return new PaginatedResult<TransactionDto>(dtos, paginationParams.PageNumber, paginationParams.PageSize, totalCount);
        }

        public async Task<List<TransactionDto>> GetByItemOfExpensesIdAsync(int itemId)
        {
            var transactions = await _transactionRepository.GetByItemOfExpensesIdAsync(itemId);
            return transactions.Select(MapToDto).ToList();
        }

        public async Task<PaginatedResult<TransactionDto>> GetByPeriodAsync(DateTime startDate, DateTime endDate, int accountId, int pageNumber = 1, int pageSize = 20)
        {
            var paginationParams = new PaginationParams { PageNumber = pageNumber, PageSize = pageSize };
            paginationParams.Validate();

            var allTransactions = await _transactionRepository.GetByPeriodAsync(startDate, endDate);
            var accountTransactions = allTransactions
                .Where(t => t.AccountId == accountId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .ToList();

            var totalCount = accountTransactions.Count;

            var paginatedTransactions = accountTransactions
                .OrderByDescending(t => t.TransactionDate)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            var dtos = paginatedTransactions.Select(MapToDto).ToList();
            return new PaginatedResult<TransactionDto>(dtos, paginationParams.PageNumber, paginationParams.PageSize, totalCount);
        }

        private TransactionDto MapToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Description = transaction.Description,
                ItemOfExpenses = new ItemOfExpensesDto
                {
                    Id = transaction.ItemOfExpenses?.Id ?? 0,
                    Name = transaction.ItemOfExpenses?.Name,
                    Type = (int)(transaction.ItemOfExpenses?.Type ?? 0)
                }
            };
        }
    }

    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly ITransactionRepository _transactionRepository;

        public BudgetService(
            IBudgetRepository budgetRepository,
            ITransactionRepository transactionRepository)
        {
            _budgetRepository = budgetRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<BudgetDto> CreateBudgetAsync(CreateBudgetDto dto)
        {
            // Check if budget already exists for this item in the same period
            var existingBudgets = await _budgetRepository.GetByCategoryIdAsync(dto.ItemOfExpensesId);
            var conflictingBudget = existingBudgets.FirstOrDefault(b =>
                !(b.EndDate < dto.StartDate || b.StartDate > dto.EndDate));

            if (conflictingBudget != null)
                throw new InvalidOperationException("Budget already exists for this item in the overlapping period.");

            var budget = new Budget
            {
                FinancialAccountId = dto.FinancialAccountId,
                ItemOfExpensesId = dto.ItemOfExpensesId,
                Amount = dto.Amount,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                SpentAmount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _budgetRepository.AddAsync(budget);

            return MapToDto(budget);
        }

        public async Task<bool> IsExceededAsync(int itemId, DateTime date)
        {
            var budget = await _budgetRepository.GetActiveBudgetAsync(itemId, date);
            if (budget == null)
                return false;

            var transactions = await _transactionRepository.GetByItemOfExpensesIdAsync(itemId);
            var spentAmount = transactions
                .Where(t => t.TransactionDate >= budget.StartDate && t.TransactionDate <= budget.EndDate)
                .Sum(t => t.Amount);

            return spentAmount > budget.Amount;
        }

        public async Task<List<BudgetDto>> GetByItemOfExpensesAsync(int itemId)
        {
            var budgets = await _budgetRepository.GetByCategoryIdAsync(itemId);
            return budgets.Select(MapToDto).ToList();
        }

        private BudgetDto MapToDto(Budget budget)
        {
            return new BudgetDto
            {
                Id = budget.Id,
                ItemOfExpensesId = budget.ItemOfExpensesId,
                Amount = budget.Amount,
                SpentAmount = budget.SpentAmount,
                StartDate = budget.StartDate,
                EndDate = budget.EndDate
            };
        }
    }
}
