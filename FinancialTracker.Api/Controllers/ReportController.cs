using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinancialTracker.Application.Interfaces;
using FinancialTracker.Api.DTO.Request.Report;
using FinancialTracker.Api.DTO.Response.Report;
using FinancialTracker.Api.DTO.Response;
using FinancialTracker.Api.DTO.Pagination;

namespace FinancialTracker.Api.Controllers;

/// <summary>
/// Контроллер для генерации отчетов.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Генерировать отчет расходов по периоду и категориям с пагинацией.
    /// </summary>
    [HttpPost("generate")]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> Generate([FromBody] GenerateReportRequest request)
    {
        try
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = 1,
                PageSize = 50
            };

            var report = await _reportService.Generate(
                request.AccountId,
                request.StartDate,
                request.EndDate,
                paginationParams
            );

            return Ok(new ApiResponse<ReportResponse>(report, "Отчет успешно сгенерирован"));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ReportResponse>(ex.Message));
        }
    }

    /// <summary>
    /// Генерировать отчет с пользовательской пагинацией.
    /// </summary>
    [HttpPost("generate/paginated")]
    public async Task<ActionResult<ApiResponse<ReportResponse>>> GenerateWithPagination(
        [FromBody] GenerateReportRequest request,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var report = await _reportService.Generate(
                request.AccountId,
                request.StartDate,
                request.EndDate,
                paginationParams
            );

            return Ok(new ApiResponse<ReportResponse>(report));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ReportResponse>(ex.Message));
        }
    }
}
