using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinancialTracker.Application.Interfaces;
using FinancialTracker.Api.DTO.Request.FinancialAccount;
using FinancialTracker.Api.DTO.Response.FinancialAccount;
using FinancialTracker.Api.DTO.Response;
using FinancialTracker.Api.DTO.Pagination;

namespace FinancialTracker.Api.Controllers;

/// <summary>
/// Контроллер для управления финансовыми счетами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FinancialAccountController : ControllerBase
{
    private readonly IFinancialAccountService _accountService;

    public FinancialAccountController(IFinancialAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Создать новый финансовый счет.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<FinancialAccountResponse>>> Create([FromBody] CreateFinancialAccountRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            
            var account = await _accountService.Create(
                request.Name,
                request.Balance,
                userId,
                request.CurrencyType,
                request.AccountType
            );

            var response = new FinancialAccountResponse
            {
                Id = account.Id,
                Name = account.Name,
                Balance = account.Balance,
                UserId = account.UserId,
                Currency = account.Currency.ToString(),
                AccountType = account.AccountType.ToString()
            };

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, 
                new ApiResponse<FinancialAccountResponse>(response, "Счет успешно создан"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<FinancialAccountResponse>(ex.Message));
        }
    }

    /// <summary>
    /// Получить счет по ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<FinancialAccountResponse>>> GetById(int id)
    {
        try
        {
            var account = await _accountService.GetById(id);

            if (account == null)
                return NotFound(new ApiResponse<FinancialAccountResponse>("Счет не найден"));

            var response = new FinancialAccountResponse
            {
                Id = account.Id,
                Name = account.Name,
                Balance = account.Balance,
                UserId = account.UserId,
                Currency = account.Currency.ToString(),
                AccountType = account.AccountType.ToString()
            };

            return Ok(new ApiResponse<FinancialAccountResponse>(response));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<FinancialAccountResponse>(ex.Message));
        }
    }

    /// <summary>
    /// Получить все счета пользователя с пагинацией.
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<FinancialAccountResponse>>>> GetByUserId(
        int userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var (accounts, totalCount) = await _accountService.GetByUserId(userId, paginationParams);

            var responses = accounts.Select(a => new FinancialAccountResponse
            {
                Id = a.Id,
                Name = a.Name,
                Balance = a.Balance,
                UserId = a.UserId,
                Currency = a.Currency.ToString(),
                AccountType = a.AccountType.ToString()
            }).ToList();

            var pagedResult = new PagedResult<FinancialAccountResponse>(
                responses,
                paginationParams.PageNumber,
                paginationParams.PageSize,
                totalCount
            );

            return Ok(new ApiResponse<PagedResult<FinancialAccountResponse>>(pagedResult));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<PagedResult<FinancialAccountResponse>>(ex.Message));
        }
    }
}
