namespace FinancialTracker.Domain;

/// <summary>
/// Отчет по доходам и расходам за период.
/// </summary>
public class Report
{
    public int Id { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}
