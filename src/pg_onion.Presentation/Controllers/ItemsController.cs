// Items Of Expenses Controller
namespace pg_onion.Presentation.Controllers
{
    using Application.DTOs;
    using Application.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly IItemOfExpensesService _itemService;

        public ItemsController(IItemOfExpensesService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Get all items of expenses
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAllItems()
        {
            try
            {
                var items = await _itemService.GetAllAsync();
                return Ok(items);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get items of expenses by type (0=Income, 1=Expense, 2=Transfer)
        /// </summary>
        [HttpGet("type/{type}")]
        public async Task<ActionResult> GetByType(int type)
        {
            try
            {
                var items = await _itemService.GetByTypeAsync(type);
                return Ok(items);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Create a new item of expenses
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ItemOfExpensesDto>> CreateItem([FromBody] ItemOfExpensesDto dto)
        {
            try
            {
                var item = await _itemService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetAllItems), item);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
