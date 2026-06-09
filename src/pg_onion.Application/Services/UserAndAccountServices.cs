// Application Service Implementations
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

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<UserDto> RegisterAsync(UserRegistrationDto dto)
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists.");

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            // Validate password (implement password hashing/verification)
            // This is a placeholder - implement proper password verification
            var token = await _authService.GenerateTokenAsync(user);
            return token;
        }
    }

    public class FinancialAccountService : IFinancialAccountService
    {
        private readonly IFinancialAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;

        public FinancialAccountService(
            IFinancialAccountRepository accountRepository,
            IUserRepository userRepository)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
        }

        public async Task<FinancialAccountDto> CreateAccountAsync(int userId, CreateFinancialAccountDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            var account = new FinancialAccount
            {
                UserId = userId,
                AccountName = dto.AccountName,
                Balance = dto.InitialBalance,
                Currency = dto.Currency,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _accountRepository.AddAsync(account);

            return new FinancialAccountDto
            {
                Id = account.Id,
                UserId = account.UserId,
                AccountName = account.AccountName,
                Balance = account.Balance,
                Currency = account.Currency,
                CreatedAt = account.CreatedAt
            };
        }

        public async Task<FinancialAccountDto> GetAccountAsync(int userId, int accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null || account.UserId != userId)
                throw new KeyNotFoundException($"Account with ID {accountId} not found for user {userId}.");

            return new FinancialAccountDto
            {
                Id = account.Id,
                UserId = account.UserId,
                AccountName = account.AccountName,
                Balance = account.Balance,
                Currency = account.Currency,
                CreatedAt = account.CreatedAt
            };
        }

        public async Task<PaginatedResult<FinancialAccountDto>> GetUserAccountsAsync(int userId, int pageNumber = 1, int pageSize = 20)
        {
            var paginationParams = new PaginationParams { PageNumber = pageNumber, PageSize = pageSize };
            paginationParams.Validate();

            var accounts = await _accountRepository.GetByUserIdAsync(userId);
            var totalCount = accounts.Count;
            
            var paginatedAccounts = accounts
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            var dtos = paginatedAccounts.Select(a => new FinancialAccountDto
            {
                Id = a.Id,
                UserId = a.UserId,
                AccountName = a.AccountName,
                Balance = a.Balance,
                Currency = a.Currency,
                CreatedAt = a.CreatedAt
            }).ToList();

            return new PaginatedResult<FinancialAccountDto>(dtos, paginationParams.PageNumber, paginationParams.PageSize, totalCount);
        }
    }
}
