using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinancialTracker.Application.Interfaces;
using FinancialTracker.Api.DTO.Request.Budget;
using FinancialTracker.Api.DTO.Response.Budget;
using FinancialTracker.Api.DTO.Response;
using FinancialTracker.Api.DTO.Pagination;
using FinancialTracker.Domain;

namespace FinancialTracker.Api.Controllers;

/// <summary>
/// Контроллер для управления бюджетами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    /// <summary>
    /// Создать новый бюджет (может быть несколько на одну категорию с разными периодами).
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<BudgetResponse>>> Create([FromBody] CreateBudgetRequest request)
    {
        try
        {
            var budget = await _budgetService.CreateBudget(
                new Budget
                {
                    Limit = request.Limit,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    AccountId = request.AccountId,
                    CategoryId = request.CategoryId,
                    Period = (BudgetPeriod)request.BudgetPeriod
                }
            );

            var response = new BudgetResponse
            {
                Id = budget.Id,
                Limit = budget.Limit,
                StartDate = budget.StartDate,
                EndDate = budget.EndDate,
                AccountId = budget.AccountId,
                CategoryId = budget.CategoryId,
                CategoryName = budget.Category.Name,
                Period = budget.Period.ToString()
            };

            return CreatedAtAction(nameof(GetById), new { id = budget.Id },
                new ApiResponse<BudgetResponse>(response, "Бюджет успешно создан"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<BudgetResponse>(ex.Message));
        }
    }

    /// <summary>
    /// Получить бюджет по ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BudgetResponse>>> GetById(int id)
    {
        try
        {
            var budget = await _budgetService.GetById(id);

            if (budget == null)
                return NotFound(new ApiResponse<BudgetResponse>("Бюджет не найден"));

            var response = new BudgetResponse
            {
                Id = budget.Id,
                Limit = budget.Limit,
                StartDate = budget.StartDate,
                EndDate = budget.EndDate,
                AccountId = budget.AccountId,
                CategoryId = budget.CategoryId,
                CategoryName = budget.Category.Name,
                Period = budget.Period.ToString()
            };

            return Ok(new ApiResponse<BudgetResponse>(response));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<BudgetResponse>(ex.Message));
        }
    }

    /// <summary>
    /// Получить все бюджеты для категории с пагинацией.
    /// </summary>
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<ApiResponse<PagedResult<BudgetResponse>>>> GetByItemOfExpenses(
        int categoryId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 25)
    {
        try
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var (budgets, totalCount) = await _budgetService.GetByItemOfExpenses(categoryId, paginationParams);

            var responses = budgets.Select(b => new BudgetResponse
            {
                Id = b.Id,
                Limit = b.Limit,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                AccountId = b.AccountId,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                Period = b.Period.ToString()
            }).ToList();

            var pagedResult = new PagedResult<BudgetResponse>(
                responses,
                paginationParams.PageNumber,
                paginationParams.PageSize,
                totalCount
            );

            return Ok(new ApiResponse<PagedResult<BudgetResponse>>(pagedResult));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<PagedResult<BudgetResponse>>(ex.Message));
        }
    }

    /// <summary>
    /// Проверить, превышен ли бюджет (сумма транзакций больше лимита).
    /// </summary>
    [HttpGet("{budgetId}/status")]
    public async Task<ActionResult<ApiResponse<BudgetStatusResponse>>> IsExceeded(int budgetId)
    {
        try
        {
            var (isExceeded, spent) = await _budgetService.IsExceeded(budgetId);
            var budget = await _budgetService.GetById(budgetId);

            if (budget == null)
                return NotFound(new ApiResponse<BudgetStatusResponse>("Бюджет не найден"));

            var response = new BudgetStatusResponse
            {
                BudgetId = budget.Id,
                CategoryName = budget.Category.Name,
                Limit = budget.Limit,
                Spent = spent,
                IsExceeded = isExceeded
            };

            return Ok(new ApiResponse<BudgetStatusResponse>(response));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<BudgetStatusResponse>(ex.Message));
        }
    }
}
