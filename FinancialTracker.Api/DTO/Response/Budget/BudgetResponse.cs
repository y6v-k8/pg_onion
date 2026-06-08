namespace FinancialTracker.Api.DTO.Response.Budget;

/// <summary>
/// Ответ с информацией о бюджете.
/// </summary>
public class BudgetResponse
{
    public int Id { get; set; }
    public decimal Limit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int AccountId { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
}
