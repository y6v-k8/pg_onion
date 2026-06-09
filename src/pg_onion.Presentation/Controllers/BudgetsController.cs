// Budgets Controller
namespace pg_onion.Presentation.Controllers
{
    using Application.DTOs;
    using Application.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        /// <summary>
        /// Create a new budget (supports multiple budgets per item with different periods)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<BudgetDto>> CreateBudget([FromBody] CreateBudgetDto dto)
        {
            try
            {
                var budget = await _budgetService.CreateBudgetAsync(dto);
                return CreatedAtAction(nameof(CreateBudget), budget);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get all budgets for a specific item of expenses
        /// </summary>
        [HttpGet("item/{itemId}")]
        public async Task<ActionResult> GetBudgetsByItem(int itemId)
        {
            try
            {
                var budgets = await _budgetService.GetByItemOfExpensesAsync(itemId);
                return Ok(budgets);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Check if budget is exceeded for an item on a specific date
        /// </summary>
        [HttpGet("item/{itemId}/check")]
        public async Task<ActionResult> CheckBudgetExceeded(int itemId, [FromQuery] DateTime date)
        {
            try
            {
                var isExceeded = await _budgetService.IsExceededAsync(itemId, date);
                return Ok(new { itemId, date, isExceeded });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
